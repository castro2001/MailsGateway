using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades.Mail
{
    public class SentMessage
    {
        public string Id { get; set; }
        public string MessageId { get; set; } // este ID lo usan los clientes de correo para "responder"
        public string Para { get; set; }
        public string Asunto { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int UsuarioId { get; set; } // el que envió

    }
}
