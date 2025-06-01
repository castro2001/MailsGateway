using Application.Interfaces;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
namespace Infrastructure.Services.Mail
{
    public class EmailConnectionProvider : IEmailConnectionProvider
    {
        private readonly IConfiguration _configuration;
        public EmailConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public SmtpClient GetSmtpClient()
        {
            var smtp = new SmtpClient();
            // ⚠️ Solo para desarrollo:
            // smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            SecureSocketOptions _secureSocketOptions = SecureSocketOptions.StartTls; // Define the security options for the SMTP connection
            smtp.Connect(
                _configuration.GetSection("Email:Host").Value,
                Convert.ToInt32(_configuration.GetSection("Email:Port").Value),
                _secureSocketOptions
            );

            //Authenticate the SMTP client using the credentials from configuration
            smtp.Authenticate(
                _configuration.GetSection("Email:Username").Value,
                _configuration.GetSection("Email:Password").Value
            );
            return smtp;
        }

        public ImapClient GetImapClient()
        {
            var client = new ImapClient();
            client.Connect(
                _configuration.GetSection("Email:Imap").Value,
                    Convert.ToInt32(_configuration.GetSection("Email:ImanPort").Value),
                 MailKit.Security.SecureSocketOptions.SslOnConnect
                    );
            client.Authenticate(
                    _configuration.GetSection("Email:Username").Value,
                    _configuration.GetSection("Email:Password").Value
                );
            return client;
        }
    }
}
