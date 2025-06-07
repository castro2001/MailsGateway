using Application.DTO;
using Application.Interfaces.Seguridad;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Seguridad
{
    public class AuthServices : IAuthServices
    {
        public void UsersList(UsuarioDTO usuario)
        {
            var nombres = usuario.Nombre + usuario.Apellido;
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, nombres),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.NameIdentifier, usuario.Perfil),
         
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties authenticationProperties = new AuthenticationProperties()
            {
                IsPersistent = true, //Persistente
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1), //Hora de expiracion
                AllowRefresh = true //Permitir refrescar el token
            };
          
        }
    }
}
