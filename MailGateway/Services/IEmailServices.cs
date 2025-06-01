using MailGateway.Models;

namespace MailGateway.Services
{
    public interface IEmailServices
    {
        EmailResponse SendEmail(EmailDTO request);
    }
}
