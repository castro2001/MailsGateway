using Domain.Entidades.Mail;

namespace Application.Interfaces
{
    public interface IEmailReaderMessageService
    {
        List<InboxMessage> LeerMensajesEnviados();
        InboxMessage DetalleMensajesEnviados(uint id);
    }
}
