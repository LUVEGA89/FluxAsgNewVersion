using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Services.Email
{
    public class Service
    {
        private string Server { get; set; }
        private string User { get; set; }
        private string Password { get; set; }
        private int Port { get; set; }
        private string From { get; set; }
        private string FromName { get; set; }

        public Service()
        {
            this.Server = ConfigurationManager.AppSettings["Email.Server"];
            this.User = ConfigurationManager.AppSettings["Email.User"];
            this.Password = ConfigurationManager.AppSettings["Email.Password"];
            this.Port = int.Parse(ConfigurationManager.AppSettings["Email.Port"]);
            this.From = ConfigurationManager.AppSettings["Email.User"];
            this.FromName = ConfigurationManager.AppSettings["Email.FromName"];
        }

        /// <summary>
        /// Description API send Email method.
        /// </summary>
        /// <param name="To">  Dirección de corre electrónico donde enviaremos el correo electrónico, podemos utilizar el metodo "add" para incluirlo.</param>        
        /// <param name="ToName">Dirección de correo electrónico desde donde se enviara el correo electrónico.</param>        
        /// <param name="Subject"> Define el título del correo electrónico..</param>     
        /// <param name="Body" >Define el cuerpo del correo electrónico.</param>
        /// <param name="Adjunto" >Definir los archivos adjutos que se le van a enviar</param>
        /// <param name="CC">Copia de los correos a enviar</param>
        /// <param name="Url"> Direccion de donde se encuentra la ruta de los archivos a enviar </param>
        /// <seealso cref="System.String">  
        /// You can use the cref attribute on any tag to reference a type or member 
        /// and the compiler will check that the reference exists.
        /// </seealso>

        
        public bool SendEmail(string To, string ToName = null, string Subject= null, string Body= null, List<string> CC = null, string Adjunto = null, string Url = null, string FromNameUrl = null)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            try
            {
                if(FromName == null)
                {
                    message.From = new MailAddress(this.From, this.FromName);
                }
                else
                {
                    message.From = new MailAddress(this.From, FromNameUrl);
                }

                
                message.To.Add(new MailAddress(To, ToName));

                if (CC != null && CC.Count >= 1)
                {
                    foreach (var item in CC)
                    {
                        message.CC.Add(new MailAddress(item));
                    }
                }

                if (FromNameUrl == "Nueva solicitud de pago a proveedores")
                {
                    message.Bcc.Add("vega.kuko89@gmail.com");
                }
                //m.CC.Add(new MailAddress("CC@yahoo.com", "Display name CC"));
                //similarly BCC
                message.Subject = Subject;
                message.IsBodyHtml = true;
                message.Body = Body;
                if (Adjunto != null)
                {
                    FileStream fs = new FileStream(Url, FileMode.Open, FileAccess.Read);
                    Attachment Adj = new Attachment(fs, Adjunto, MediaTypeNames.Application.Octet);
                    message.Attachments.Add(Adj);
                }
                
                smtp.Host = this.Server;
                smtp.Port = this.Port;
                message.Priority = MailPriority.High;
                smtp.Credentials = new NetworkCredential(this.User, this.Password);
                //sc.EnableSsl = true;
                smtp.Send(message);
                smtp.Dispose();
                message.Dispose();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        /*
        public bool SendEmail(string To, string ToName = null, string Subject = null, string Body = null, List<string> CC = null, string Adjunto = null, string Url = null, string FromNameUrl = null)
        {
            // Replace sender@example.com with your "From" address. 
            // This address must be verified with Amazon SES.
            String FROM = this.User;
            String FROMNAME = "";

            if (FromName == null)
            {
                FROMNAME = this.FromName ?? "";
            }
            else
            {
                FROMNAME = FromNameUrl ?? "";
            }


            // Replace recipient@example.com with a "To" address. If your account 
            // is still in the sandbox, this address must be verified.
            //String TO = "vega.kuko89@gmail.com";

            // Replace smtp_username with your Amazon SES SMTP user name.
            //String SMTP_USERNAME = "AKIARHUSVANZQA25EOCB";
            String SMTP_USERNAME = "noreply@fussionweb.com";

            // Replace smtp_password with your Amazon SES SMTP password.
            //String SMTP_PASSWORD = "BPYcGf3g4H5wjOdpGMvZDTzRKBk0R/ZcvNvGT6SyvwPA";
            String SMTP_PASSWORD = "QBrSoWmXCb2c:a6MBVlKH6VofDsiHaSfWKLH8q";

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            //String CONFIGSET = "ConfigSet";

            // If you're using Amazon SES in a region other than US West (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate AWS Region.
            String HOST = "email-smtp.us-east-1.amazonaws.com";

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            int PORT = 587;

            // The subject line of the email
            //String SUBJECT = "Prueba (SMTP usando C#)";

            // The body of the email
            //String BODY =
            //    "<h1>Prueba</h1>" +
            //    "<p>cuerpo del correo " +
            //    "SMTP interface " +
            //    "usando .NET System.Net.Mail library.</p>";

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(To, ToName));
            if (CC != null && CC.Count >= 1)
            {
                foreach (var item in CC)
                {
                    message.CC.Add(new MailAddress(item));
                }
            }

            if (FromNameUrl == "Nueva solicitud de pago a proveedores")
            {
                message.Bcc.Add("vega.kuko89@gmail.com");
            }
            message.IsBodyHtml = true;
            message.Subject = Subject;
            message.Body = Body;
            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }*/
    }
}
