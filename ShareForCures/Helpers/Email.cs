using Newtonsoft.Json;
using ShareForCures.Models.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace ShareForCures.Helpers
{
    public class Email
    {
        public bool SendEmail(string emailTo, string subject, string body)
        {
             var   emailFrom = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"];
            MailMessage msg = null;


            // Supply your SMTP credentials below. Note that your SMTP credentials are different from your AWS credentials.
            String smtpUserName = System.Configuration.ConfigurationManager.AppSettings["SMTPUserName"]; // Replace with your SMTP username. 
            String smtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];  // Replace with your SMTP password.

            // Amazon SES SMTP host name. This example uses the US West (Oregon) region.
            String host = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"];

            // The port you will connect to on the Amazon SES SMTP endpoint. We are choosing port 587 because we will use
            // STARTTLS to encrypt the connection.
            int port =Convert.ToInt16( System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);

            // Create an SMTP client with the specified host name and port.
            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(host, port))
            {
                // Create a network credential with your SMTP user name and password.
                client.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
                
                // Use SSL when accessing Amazon SES. The SMTP session will begin on an unencrypted connection, and then 
                // the client will issue a STARTTLS command to upgrade to an encrypted connection using SSL.
                client.EnableSsl = true;

                // Send the email. 
                try
                {
                    msg = new MailMessage(emailFrom, emailTo, subject, body);
                    msg.IsBodyHtml = true;
                    client.Send(msg);
                    return true;
                }
                catch (Exception ex)
                {
                    //Insert Error Log
                    var waErrorLog = new WAErrorLog
                    {
                        ErrTypeID = 2,
                        ErrSourceID = 4,
                        Code = ex.HResult.ToString(),
                        Description = "Email Send: " + ex.Message,
                        Trace = ex.StackTrace,
                        CreateDateTime = DateTime.Now
                    };
                    var errClient = new HttpClient { BaseAddress = new Uri(Service.REST_API_SERVER) };
                    var json = JsonConvert.SerializeObject(waErrorLog);

                    var result = errClient.PostAsync(Service.ADD_WEBAPP_ERROR, new StringContent(json, Encoding.UTF8, "application/json"));

                    return false;
                }
            }
        }
    }
}