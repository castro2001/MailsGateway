using Domain.Entidades.Mail;

namespace Application.Interfaces
{
    public interface IEmailReaderService
    {
        List<Email> LeerMensajesRecibidos();
        Email DetalleCorreo(uint id);
    }
}
