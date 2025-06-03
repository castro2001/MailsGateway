using Application.DTO;
using Domain.Entidades.Mail;


namespace Application.Interfaces
{
    public interface INotificationStore
    {


         void Agregar <T>(T noti);

        List<T> Obtener<T>(Func<object, T> convert);
        void Limpiar();

        bool VerificarYNotificarRespuesta(string inReplyTo, string remitente);



    }
}
