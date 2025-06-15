using Application.DTO;
using Application.Interfaces;
using Domain.Entidades.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Mail
{
    public class NotificationStoreServices : INotificationStore
    {
        private static ConcurrentBag<object> _notificaciones = new ConcurrentBag<object>();
        // Lista de mensajes enviados que usas para verificar respuestas
        private List<SentMessage> _mensajesEnviados = new List<SentMessage>();
        public void SetConcurrenct(ConcurrentBag<object> notificaciones)
        {
            _notificaciones = notificaciones;
        }
        public void Agregar<T>(T noti)
        {
            _notificaciones.Add(noti!);
        }

        public List<T> Obtener<T>(Func<object, T> convert)
        {
            return _notificaciones.Select(convert).ToList();
        }
        public void Limpiar()
        {
            _notificaciones = new ConcurrentBag<object>();
        }



        // Método para verificar si un mensaje recibido es respuesta a uno enviado
        public bool VerificarYNotificarRespuesta(string inReplyTo, string remitente)
        {
            if (string.IsNullOrWhiteSpace(inReplyTo)) return false;

            // Buscar en notificaciones que son mensajes enviados
            var mensajeOriginal = _notificaciones
                .OfType<EmailResponse>()
                .FirstOrDefault(n => n.MessageId == inReplyTo);

            if (mensajeOriginal != null)
            {
                // Aquí creas el registro de mensaje respondido (SentMessage)
                Agregar(new SentMessage
                {
                    Uid = Guid.NewGuid().ToString(), // Genera un nuevo ID único
                    MessageId = inReplyTo,
                    Para = remitente, // El remitente de la respuesta
                    Asunto = mensajeOriginal.Asunto, // Asunto del mensaje original
                    FechaEnvio = DateTime.Now, // Fecha actual como fecha de respuesta
                    //UsuarioId = 0 // Aquí puedes asignar el ID del usuario que envió originalmente si es necesario
                });

                return true;
            }

            return false;
        }



    }
}
