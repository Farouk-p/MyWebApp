using MyWebApp.DbContextData;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;

namespace MyWebApp.ApiServices
{
    public class EmailService : IEmailService
    {
        private readonly MyWebAppDbContext _context;
        private readonly IConfiguration _config;

        //Gain access to App settings
        public EmailService(MyWebAppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> SendMailAsync(string name, string emailAddress, string body, string subject)
        {

            try
            {
                //store info needed from Appsettings in a variable
                var senderMail = _config.GetSection("SenderMail").Value;
                var mailPassword = _config.GetSection("MailPassword").Value;
                var smtpGmailHost = _config.GetSection("SmtpGmailHost").Value;
                var smtpGmailPort = int.Parse(_config.GetSection("SmtpGmailPort").Value);

                //Get Email Template From Database
                //var getSpecificEmail = _context.emailTemplate.FirstOrDefault(x => x.ID == 1);
                var emailFormat = body;

                //Replace Email Format with receiver Email
                emailFormat = emailFormat.Replace("{EmailAddress}", name);

                //Generate 6 digit OTP 
                Random random = new Random();
                int randomDigit = random.Next(1000000);
                var sixDigits = randomDigit.ToString();

                //Replace Email format with generate OTP
                emailFormat = emailFormat.Replace("{sixDigits[0]}", sixDigits[0].ToString());
                emailFormat = emailFormat.Replace("{sixDigits[1]}", sixDigits[1].ToString());
                emailFormat = emailFormat.Replace("{sixDigits[2]}", sixDigits[2].ToString());
                emailFormat = emailFormat.Replace("{sixDigits[3]}", sixDigits[3].ToString());
                emailFormat = emailFormat.Replace("{sixDigits[4]}", sixDigits[4].ToString());
                emailFormat = emailFormat.Replace("{sixDigits[5]}", sixDigits[5].ToString());



                MailMessage message = new MailMessage();
                message.Subject = subject;
                message.IsBodyHtml = true;

                message.Body = @emailFormat;

                //since email is a list, loop through email that will be passed from rwquest
                //foreach (var email in emailAddresses)
                //{
                //    message.To.Add(new MailAddress(email));
                //}

                message.To.Add(new MailAddress(emailAddress));

                message.From = new MailAddress(senderMail);


                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new NetworkCredential(senderMail, mailPassword);

                smtpClient.Port = smtpGmailPort;
                smtpClient.Host = smtpGmailHost;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;

                smtpClient.Send(message);

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
