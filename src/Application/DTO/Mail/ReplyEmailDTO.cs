using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Mail
{
    public class ReplyEmailDTO : EmailMessageBase
    {
        // Contenido de la respuesta al correo
        public string ContenidoRespuesta { get; set; } = string.Empty!; 
        public string EncryptedUid { get; set; } = string.Empty!; // UID encriptado del correo original
        public bool IsForwarded { get; set; } = false; // Indica si es un reenvío o una respuesta
    }
}
