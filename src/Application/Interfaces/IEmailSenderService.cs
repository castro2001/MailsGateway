using Application.DTO;
using Application.DTO.Mail;
using Domain.Entidades.Mail;

namespace Application.Interfaces
{
    public interface IEmailSenderService
    {
        EmailResponse SendEmail(ComposeEmailDto request);
        EmailResponse ReenviarCorreo(ForwardEmailDto mensaje);
        EmailResponse ResponderCorreo(ReplyEmailDTO mensaje);

    }
}
