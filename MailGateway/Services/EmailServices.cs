using MailGateway.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;


namespace MailGateway.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public EmailResponse SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();//Create a new MimeMessage instance
            
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:Username").Value));//Add the sender's email address from configuration
            email.To.Add(MailboxAddress.Parse(request.Para));
            email.Subject = request.Asunto;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Contenido };
            try
            {
                //Conecto CON EL CLIENTE SMTP PARA ENVIAR EL CORREO
                using var smtp = new SmtpClient();
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

                smtp.Send(email); //Send the email
                smtp.Disconnect(true); //Disconnect from the SMTP server
                return new EmailResponse
                {
                    Success = true,
                    MessageId = email.MessageId // Return the message ID of the sent email
                };
            }
            catch (Exception ex)
            {
                return new EmailResponse
                {
                    Success = true,
                    MessageId = ex.Message // Return the message ID of the sent email
                };
            
            }



        }
    }
}
