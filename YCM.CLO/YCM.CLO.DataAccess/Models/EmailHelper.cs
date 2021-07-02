using System.Configuration;
using System.Linq;
using System.Net.Mail;

namespace YCM.CLO.DataAccess.Models
{
	public class EmailHelper 
    {
        public static void SendEmail(string message, string subject)
        {
            var msg = new MailMessage();
            var emailTos = ConfigurationManager.AppSettings["SupportToEmailIds"].Split(new char[] { ',',';' });
            emailTos.ToList().ForEach(e =>
            {
                msg.To.Add(e);
            });
            msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyToEmail"], ConfigurationManager.AppSettings["ReplyToName"]));

            msg.From = new MailAddress(ConfigurationManager.AppSettings["CLOSupportEmail"], ConfigurationManager.AppSettings["CLOSupportName"]);
            msg.Subject = subject;
            msg.Body = message;
            SmtpClient smtpClient = new SmtpClient();
            //smtpClient.Send(msg);
        }
    }
}
