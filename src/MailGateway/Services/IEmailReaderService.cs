using MailGateway.Models;

namespace MailGateway.Services
{
    public interface IEmailReaderService
    {
        List<EmailDTOa> LeerMensajesRecibidos();
        EmailDTOa DetalleCorreo(uint id);
    }
}
