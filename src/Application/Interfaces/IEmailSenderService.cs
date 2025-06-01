using Domain.Entidades.Mail;
using Application.DTOS;

namespace Application.Interfaces
{
    public interface IEmailSenderService
    {
        EmailResponse SendEmail(EmailDTO request);
    }
}
