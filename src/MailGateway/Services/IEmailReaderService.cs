using MailGateway.Models;

namespace MailGateway.Services
{
    public interface IEmailReaderService
    {
        List<EmailDTO> LeerMensajesRecibidos();
        EmailDTO DetalleCorreo(uint id);
    }
}
