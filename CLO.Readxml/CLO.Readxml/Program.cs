using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;

namespace CLO.Readxml
{
    class Program
    {
        private readonly static NLog.ILogger _logger = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {


                var xmldoc = new XmlDataDocument();
                XmlNodeList xmlnode;
                string TradeGroupId = "";
                string TradeId = "";
                string strStatus = "";
                string strMessage = "";
                string strXMLdoc = "";
                var connectionString = ConfigurationManager.ConnectionStrings["Yoda"].ConnectionString;
                var folderpath = ConfigurationManager.AppSettings["WSOXMLResponse"];
                var archivalfolderpath = ConfigurationManager.AppSettings["WSOXMLResponseArchive"];
                string filename = "";

                foreach (string file in Directory.EnumerateFiles(folderpath, "*.xml"))
                {
                    bool IsCancelledTrade = false;
                    filename = Path.GetFileName(file);
                    _logger.Info(filename + " reading started.");
                    xmldoc = new XmlDataDocument();
                    TradeGroupId = "";
                    TradeId = "";
                    strStatus = "";
                    strMessage = "";
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        xmldoc.Load(fs);
                        strXMLdoc = xmldoc.OuterXml;
                        xmlnode = xmldoc.GetElementsByTagName("ASSET");
                        foreach (XmlNode nodes in xmlnode)
                        {
                            for (int i = 0; i < nodes.ChildNodes.Count; i++)
                            {
                                if (nodes.ChildNodes[i].Name == "TRADEGROUP")
                                {
                                    TradeGroupId = nodes.ChildNodes[i].Attributes[1].Value;
                                }
                                if (nodes.ChildNodes[i].Name == "TRADE")
                                {
                                    TradeId = nodes.ChildNodes[i].Attributes[1].Value;
                                }
                            }
                        }
                        xmlnode = xmldoc.GetElementsByTagName("STATUS");
                        foreach (XmlNode nodes in xmlnode)
                        {
                            for (int intnodes = 0; intnodes < nodes.ChildNodes.Count; intnodes++)
                            {
                                if (nodes.ChildNodes[intnodes].Name == "ERROR")
                                {
                                    foreach (XmlNode innernodes in nodes.ChildNodes)
                                    {
                                        strStatus = "Error";
                                        if (innernodes.ChildNodes.Count > 0)
                                            strMessage += innernodes.ChildNodes[intnodes].Attributes[0].Value + " ";
                                        else
                                            strMessage += nodes.ChildNodes[intnodes].Attributes[0].Value;
                                    }
                                }
                                else if (nodes.ChildNodes[intnodes].Name == "MESSAGE")
                                {
                                    strStatus = "Message";
                                    strMessage += nodes.ChildNodes[intnodes].Attributes[0].Value;
                                    if (strMessage.Contains("Cancel affected the following trades"))
                                    {
                                        IsCancelledTrade = true;
                                    }
                                }
                                else if (nodes.ChildNodes[intnodes].Name == "WARNING")
                                {
                                    strStatus = "Warning";
                                    strMessage += nodes.ChildNodes[intnodes].Attributes[0].Value;
                                }
                                else if (nodes.ChildNodes[intnodes].Name == "COMPLETE")
                                {
                                    strStatus = "Complete";
                                    strMessage += nodes.ChildNodes[intnodes].Attributes[0].Value;
                                }
                            }
                        }
                        _logger.Info(filename + " reading completed.");
                        if (TradeGroupId.Trim() != "" && TradeId.Trim() != "" && strStatus.Trim() != "")
                        {
                            UpdateTradeResponse(TradeGroupId, TradeId, strStatus, strMessage, strXMLdoc, connectionString, IsCancelledTrade);
                        }
                    }
                    //if (File.Exists(file))
                    //{
                    //    _logger.Info(filename + " moving file to archival started.");
                    //    File.Move(file, archivalfolderpath + "\\" + filename);
                    //    _logger.Info(filename + " moving file to archival completed.");
                    //}
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message.ToString());
            }
        }

        private static void UpdateTradeResponse(string TradeGroupId, string TradeId, string strStatus, string strMessage, string strXMLdoc, string connectionString, bool IsCancelledTrade)
        {
            bool filexist = false;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (IsCancelledTrade)
                {
                    filexist = false;
                    strStatus = "Cancelled";
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand("CLO.dbsp_CheckDuplicateResponse", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TradeGroupId", TradeGroupId));
                        cmd.Parameters.Add(new SqlParameter("@TradeId", TradeId));
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                            filexist = true;
                    }
                }

                if (filexist == false)
                {
                    _logger.Info(" updating in database.");
                    using (SqlCommand cmd = new SqlCommand("CLO.dbsp_InsertTradeBookingResponse", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TradeGroupId", TradeGroupId));
                        cmd.Parameters.Add(new SqlParameter("@TradeId", TradeId));
                        cmd.Parameters.Add(new SqlParameter("@Response", strXMLdoc));
                        cmd.Parameters.Add(new SqlParameter("@ResponseStatus", strStatus));
                        cmd.Parameters.Add(new SqlParameter("@ErrorMessage", strMessage));
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e);
                        }
                    }
                    _logger.Info(" updating in database completed.");
                }

                if (strStatus == "Error")
                {
                    SendErrorEmail(TradeId, connectionString);
                }

            }
        }

        private static void SendErrorEmail(string TradeId,string connectionString)
        {
            try
            {
                _logger.Info("SendErrorEmail started");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("CLO.dbsp_GetTradeBookingDetails", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TradeId", TradeId));
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {

                                // rdr.HasRows is redundant - rdr.Read() returns true if record has been read
                                if (rdr.Read())
                                {
                                    String key = Convert.ToString(rdr["Id"]);
                                    // Put break point here: what is the "key" value?

                                }

                                string message = File.ReadAllText(ConfigurationManager.AppSettings["HtmlFilePath"]);
                                message = message.Replace("{issuer}", Convert.ToString(rdr["IssuerDesc"]));
                                message = message.Replace("{partyName}", Convert.ToString(rdr["PartyName"]));
                                message = message.Replace("{loanxId}", Convert.ToString(rdr["LoanXId"]));
                                message = message.Replace("{totalQty}", Convert.ToString(rdr["TotalQty"]));
                                message = message.Replace("{price}", Convert.ToString(rdr["Price"]));
                                message = message.Replace("{setType}", Convert.ToString(rdr["SettleMethod"]));
                                message = message.Replace("{tradeTypeDesc}", Convert.ToString(rdr["TradeTypeDesc"]));
                                message = message.Replace("{newTradeDate}", Convert.ToString(rdr["NewTradeDate"]));
                                message = message.Replace("{errorMessage}", Convert.ToString(rdr["ErrorMessage"]));
                                



                                string subject = ConfigurationManager.AppSettings["SendErrorEmailURLSubject"];
                                //string html = File.ReadAllText("..\\..\\SendError.html");
                                subject = subject.Replace("{issuer}", Convert.ToString(rdr["IssuerDesc"]));
                                Console.WriteLine(message);
                                _logger.Info("Email Message to be sent:" + message);

                                var msg = new MailMessage();
                               msg.IsBodyHtml = true;
                                //msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));
                                _logger.Info("Email Message object created");

                                var emailTos = ConfigurationManager.AppSettings["SendErrorEmailToEmailIds"].Split(new char[] { ',', ';' });
                                _logger.Info("Email Message to be sent to:" + emailTos);
                                emailTos.ToList().ForEach(e =>
                                {
                                    msg.To.Add(e);
                                });

                                string ccList = ConfigurationManager.AppSettings["SendErrorEmailCCEmailIds"];
                                _logger.Info("Email Message to be CC:" + ccList);
                                if (!string.IsNullOrEmpty(ccList))
                                {
                                    ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
                                    {
                                        msg.CC.Add(e);
                                    });
                                }

                                msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
                                msg.Subject = subject;
                                msg.Body = message;

                                _logger.Info("SendErrorEmail ~~ message:" + message + "  ;From:" + msg.From + " ;Subject:" + msg.Subject);
                                SmtpClient smtpClient = new SmtpClient();
                                smtpClient.Send(msg);
                                _logger.Info("Send Error Email Sent successfully");
                            }
                        }

                    }
                }

                


            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
                _logger.Info("Exeption while sending email message on SendErrorEmail");
                _logger.Error(exception);

            }
        }
    }
}
