using Domain.Entidades.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Mail
{
    public class EmailMessageBase
    {
        public string Uid { get; set; } = string.Empty!;// UID del correo original, no encriptado

        public string De { get; set; } = string.Empty!; 
        public string Para { get; set; } = string.Empty!;
        public string Asunto { get; set; } = string.Empty!;
        public string Contenido { get; set; } = string.Empty!;

        public string Perfil { get; set; } = string.Empty!;

        public DateTime Fecha { get; set; } = DateTime.Now;
        public string FechaFormateada { get; set; } = string.Empty!;

        public string EnviadorPor { get; set; } = string.Empty!;
        public string FirmadoPor { get; set; } = string.Empty!;
        // Nivel de seguridad (TLS, etc.)
        public string Seguridad { get; set; } = string.Empty!;
        public List<MensajeAdjunto> Adjuntos { get; set; } = new(); // nuevos adjuntos


    }
}
/*
 Asunto
    Fecha
Perfil
De 
Para 
Contendio 

EnviadorPor 
FirmadoPor
Seguridad
Adjuntos Lista de clase adjuntos
ContenidoRespuesta

 */