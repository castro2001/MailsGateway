using Application.DTO;
using System.Security.Claims;

namespace Application.Interfaces.Seguridad
{
    public interface IAuthServices
    {
        public void UsersList(UsuarioDTO usuario);
    }
}
