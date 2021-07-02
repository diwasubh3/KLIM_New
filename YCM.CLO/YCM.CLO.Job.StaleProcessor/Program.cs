using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.Job.StaleProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
 //           try
//            {
//               Console.WriteLine("Starting");

            //                IRepository repository = new Repository();
            //                //repository.CreateStalePositions();

            //                WebClient client = new WebClient();
            //                string message = client.DownloadString(ConfigurationManager.AppSettings["URL"]);
            //                string subject = ConfigurationManager.AppSettings["Subject"];

            //                subject = subject.Replace("{date}", DateTime.Today.ToShortDateString());
            //                Console.WriteLine(message);

            //                var msg = new MailMessage();
            //                msg.Body = message;
            //                msg.IsBodyHtml = true;
            //                msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));

            //                var emailTos = ConfigurationManager.AppSettings["ToEmailIds"].Split(new char[] { ',', ';' });
            //                emailTos.ToList().ForEach(e =>
            //                {
            //                    msg.To.Add(e);
            //                });

            //                string ccList = ConfigurationManager.AppSettings["CCEmailIds"];
            //                if (!string.IsNullOrEmpty(ccList))
            //                {
            //                    ccList.Split(new char[] { ',', ';' }).ToList().ForEach(e =>
            //                    {
            //                        msg.CC.Add(e);
            //                    });
            //                }

            //                msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
            //                msg.Subject = subject;
            //                msg.Body = message;
            //                SmtpClient smtpClient = new SmtpClient();
            //                smtpClient.Send(msg);

            //                Console.WriteLine("Finished");
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex.ToString());
            //                throw;
            //            }

            }
    }
    }
