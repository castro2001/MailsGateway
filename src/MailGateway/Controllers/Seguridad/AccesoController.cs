using Application.DTO;
using Application.DTO.Usuarios;
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
        public async Task<IActionResult> Login( LoginDTO usuarioDTO)
        {
            var usuarioValidate = await _usuarioRepository.ValidarUsuarioAsync(usuarioDTO);
            if (usuarioValidate != null)
            {
                var usuario = new Usuario
                {
                    Id = usuarioValidate.Id,
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarioDTO);

            }




            if (usuarioDTO.Imagen != null && usuarioDTO.Imagen.Length > 0)
            {
                var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(usuarioDTO.Imagen.FileName).ToLowerInvariant();

                if (!extensionesPermitidas.Contains(extension))
                {
                    ModelState.AddModelError(nameof(usuarioDTO.Imagen), "Solo se permiten imágenes JPG, JPEG o PNG.");
                  
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

            if (!resultado.Exito)
            {
                ViewBag.Message = resultado.Message;
            }
            else
            {
                ViewBag.Message = $"Error al guardar el registro: {resultado.Message}";
            }


            return View(new RegistroDTO());
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

    }
}
