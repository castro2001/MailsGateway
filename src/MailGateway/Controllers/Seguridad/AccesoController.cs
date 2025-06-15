using Application.DTO;
using Application.Interfaces.Seguridad;
using Domain.Entidades.Seguridad;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using Shared.Helper;

namespace MailGateway.Controllers.Seguridad
{
    public class AccesoController : Controller
    {
        private readonly UsuarioRepositoy _usuarioRepository;
        private readonly IAuthServices _authServices;

        public AccesoController(UsuarioRepositoy usuarioRepository, IAuthServices authServices)
        {
            _usuarioRepository = usuarioRepository;
            _authServices = authServices;
        }

        [HttpGet]

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Perfil", "Perfil");
            return View();
        }

        

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login( UsuarioDTO usuarioDTO)
        {
            var usuarioValidate = await _usuarioRepository.ValidarUsuarioAsync(usuarioDTO);
            if (usuarioValidate != null)
            {
                var usuario = new Usuario
                {
    
                    Nombre = usuarioValidate.Nombre,
                    Apellido = usuarioValidate.Apellido,
                    CorreoElectronico = usuarioValidate.CorreoElectronico,
                    Perfil = usuarioValidate.Perfil
                };
                await _authServices.AutenticarUsuarioAsync(usuario);
                return RedirectToAction("Index", "Home");
            }
            ViewData["message"] = "Usuario o Contraseña incorrectos";
            RedirectToAction();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioDTO usuarioDTO)
        {
            if (!SecurityHelper.EsClaveSegura(usuarioDTO.Clave))
            {
                ViewBag.Message = "La contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, números y símbolos.";
                return View();
            }

            if (usuarioDTO.Clave != usuarioDTO.RepetirClave)
            {
                ViewBag.Message = "Las contraseñas no coinciden.";
                return View();
            }

            if (!SecurityHelper.EsCorreoGmailValido(usuarioDTO.CorreoElectronico))
            {
                ViewBag.Message = "El correo electrónico debe ser una cuenta de Gmail válida.";
                return View();
            }

            if (usuarioDTO.Imagen != null && usuarioDTO.Imagen.Length > 0)
            {
                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(usuarioDTO.Imagen.FileName).ToLowerInvariant();

                if (!extensionesPermitidas.Contains(extension))
                {
                    ViewBag.Message = "Solo se permiten imágenes JPG, JPEG o PNG.";
                    return View();
                }

                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var nombreUnico = $"Perfil_{timestamp}{extension}";

                var rutaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagen/Perfil/", nombreUnico);
                Directory.CreateDirectory(Path.GetDirectoryName(rutaDestino)!);

                using (var stream = new FileStream(rutaDestino, FileMode.Create))
                {
                    usuarioDTO.Imagen.CopyTo(stream);
                }

                usuarioDTO.Perfil = nombreUnico;
            }

            var resultado = await _usuarioRepository.crearUsuario(usuarioDTO);

            if (resultado.Exito)
            {
                ViewBag.Message = resultado.Message;
            }
            else
            {
                ViewBag.Message = $"Error al guardar el registro: {resultado.Message}";
            }

            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

    }
}
