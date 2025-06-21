using Domain.Entidades.Mail;

namespace Application.Interfaces
{
    public interface IEmailReaderService
    {
       Task< List<InboxMessage> >LeerMensajesRecibidos();
        InboxMessage DetalleCorreo(uint id);
    }
}
