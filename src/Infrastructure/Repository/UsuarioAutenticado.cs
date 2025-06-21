using Domain.Entidades.Seguridad;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UsuarioAutenticado
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UsuarioAutenticado(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Usuario? ObtenerUsuario()
        {
            var user = _contextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                return null;
            var idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var  userId = int.TryParse(idClaim?.Value, out var parsedId) ? parsedId : 0; 
            var emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var nameClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var profileClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality);
            if (idClaim == null || emailClaim == null || nameClaim == null || profileClaim == null)
                return null;
            return new Usuario
            {
                Id = userId,
                CorreoElectronico = emailClaim.Value,
                Nombre = nameClaim.Value.Split(' ')[0],
                Apellido = nameClaim.Value.Split(' ').Skip(1).FirstOrDefault() ?? string.Empty,
                Perfil = profileClaim.Value
            };
        }



    }
}
