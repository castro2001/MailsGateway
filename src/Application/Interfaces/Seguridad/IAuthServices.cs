using Domain.Entidades.Seguridad;


namespace Application.Interfaces.Seguridad
{
    public interface IAuthServices
    {
        public  Task AutenticarUsuarioAsync(Usuario usuario);
    }
}
