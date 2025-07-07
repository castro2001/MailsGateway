using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Mail
{
    public class ForwardEmailDto : EmailMessageBase
    {
        // Correo del nuevo destinatario al que se reenvía el mensaje
        public string NuevoDestinatario { get; set; } = string.Empty!;
        // Contenido de la respuesta al correo
        public string ContenidoRespuesta { get; set; } = string.Empty!;

    }
}
