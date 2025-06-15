using Domain.Entidades.Mail;

namespace Application.Interfaces
{
    public interface IEmailReaderService
    {
        List<InboxMessage> LeerMensajesRecibidos();
        InboxMessage DetalleCorreo(uint id);
    }
}
