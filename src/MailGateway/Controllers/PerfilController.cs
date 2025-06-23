using Application.DTO;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helper;
using System.Security.Claims;
namespace MailGateway.Controllers
{
    //[Authorize]
    public class PerfilController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;
        public PerfilController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        [HttpGet]
        public IActionResult Perfil()
        {
            var correo = User.Claims
                .Where(claim => claim.Type == ClaimTypes.Email)
                .Select(claim => claim.Value)
                .SingleOrDefault();

            if (string.IsNullOrEmpty(correo))
            {
                return RedirectToAction("Login", "Account");
            }

            var usuario = _usuarioRepository.obtenerUsuarioCorreo(correo).Result;

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Mapea a DTO si es necesario
            var usuarioDTO = new UsuarioDTO
            {
               // Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                CorreoElectronico = usuario.CorreoElectronico,
                Clave = usuario.Clave,
                Perfil = usuario.Perfil,
                LlaveSecreta = usuario.PasswordSecret,
                // otros campos si tienes
            };

            return View(usuarioDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Perfil(UsuarioDTO usuarioDTO)
        {


            if (string.IsNullOrWhiteSpace(usuarioDTO.LlaveSecreta) || usuarioDTO.LlaveSecreta.Length > 16)
            {
                ViewData["message"] = "Debe añadir su contraseña de aplicación (máx. 16 caracteres).No debe contener espacios";
                return View();
            }


            var resultado = await _usuarioRepository.ActualizarPasswordSecret(usuarioDTO);

            if (resultado.Exito)
            {
                ViewData["message"] = resultado.Message;
                return View();
            }
            else
            {
                ViewData["message"] = $"Error al guardar el registro: {resultado.Message}";
                return View();
            }

            return View();
        }

    }
}
