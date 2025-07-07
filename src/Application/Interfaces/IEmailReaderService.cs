using Domain.Entidades.Mail;

namespace Application.Interfaces
{
    public interface IEmailReaderService
    {
        List<InboxMessage> LeerMensajesRecibidos(out string errorMessage);
        InboxMessage DetalleCorreo(uint id);
    }
}
