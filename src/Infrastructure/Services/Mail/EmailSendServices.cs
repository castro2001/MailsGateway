using Application.Interfaces;
using Domain.Entidades.Mail;
using Application.DTOS;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;


namespace Infrastructure.Services.Mail
{
    public class EmailSendServices : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailConnectionProvider _emailConnectionProvider;

        public EmailSendServices(IConfiguration configuration, IEmailConnectionProvider emailConnectionProvider)
        {
            _configuration = configuration;
            _emailConnectionProvider = emailConnectionProvider;
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
                using var smtp = _emailConnectionProvider.GetSmtpClient();
                smtp.Send(email);
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
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
