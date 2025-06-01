using Domain.Entidades.Mail;

namespace Application.Interfaces
{
    public interface IEmailReaderMessageService
    {
        List<Email> LeerMensajesEnviados();
        Email DetalleMensajesEnviados(uint id);
    }
}
