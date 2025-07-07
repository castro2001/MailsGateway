using Application.DTO;
using Application.DTO.Mail;
using Application.Interfaces;
using Domain.Entidades.Mail;
using MailGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helper;

namespace MailGateway.Controllers
{
    //[Authorize]
    public class RedactarController : Controller
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly CryptoHelper _cryptoHelper;

        public RedactarController(IEmailSenderService emailSenderService, CryptoHelper cryptoHelper)
        {
            _emailSenderService = emailSenderService;
            _cryptoHelper = cryptoHelper;
        }
        [HttpGet]
        public IActionResult Redactar()
        {
     
            return View();
        }

        [HttpPost]
        public IActionResult Redactar(ComposeEmailDto emailDAO)
        {
            if (string.IsNullOrWhiteSpace(emailDAO.Para))
            {
                ViewBag.Error = "El campo Para es obligatorio";
                return View();
            }

            if (string.IsNullOrWhiteSpace(emailDAO.Asunto))
            {
                ViewBag.Error = "El campo Asunto es obligatorio";
                return View();
            }

            if (string.IsNullOrWhiteSpace(emailDAO.Contenido))
            {
                ViewBag.Error = "El campo Contenido es obligatorio";
                return View();
            }

            var email = new ComposeEmailDto
            {
                Para = emailDAO.Para,
                Asunto = emailDAO.Asunto,
                Contenido = emailDAO.Contenido,
                Imagenes = emailDAO.Imagenes ,// pasa las imágenes también,
                ArchivosAdjuntos = emailDAO.ArchivosAdjuntos
            };

            var resultado = _emailSenderService.SendEmail(email);

            if (resultado.Success)
            {
                ViewBag.Success = "Correo enviado correctamente.";
                return View(new EmailDTO());
            }
            else
            {
                ViewBag.Error = $"Error al enviar correo: {resultado.ErrorMessage}";
                return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reenviar(ForwardEmailDto emailDAO)
        {
            // InboxMessage
            if (string.IsNullOrWhiteSpace(emailDAO.ContenidoRespuesta))
            {
                ViewBag.Error = "El campo Contenido Respuesta  es obligatorio";
                return View("Redactar");
            }
            string uidDesencriptado = _cryptoHelper.Decrypt(emailDAO.Uid);

            var email = new ForwardEmailDto
            {
                Uid = uidDesencriptado,
                Para = emailDAO.Para,
                Asunto = emailDAO.Asunto,
                ContenidoRespuesta = emailDAO.ContenidoRespuesta,
                NuevoDestinatario = emailDAO.NuevoDestinatario,
                Adjuntos = emailDAO.Adjuntos, // Asegúrate de pasar los adjuntos si es necesario

            };

            var resultado = _emailSenderService.ReenviarCorreo(email);

            if (resultado.Success)
            {
                ViewBag.Success = "Correo enviado correctamente.";
                return View("Redactar",new ComposeEmailDto());
            }
            else
            {
                ViewBag.Error = $"Error al enviar correo: {resultado.ErrorMessage}";
                return View("Redactar");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Responder(ReplyEmailDTO emailDAO)
        {
            if ( string.IsNullOrWhiteSpace(emailDAO.ContenidoRespuesta))
            {
                ViewBag.Error = "El campo Contenido Respuesta  es obligatorio.";
                return View(emailDAO);
            }
            string uidDesencriptado = _cryptoHelper.Decrypt(emailDAO.Uid);
            var email = new ForwardEmailDto
            {
                Uid = uidDesencriptado,
                Para = emailDAO.Para,
                Asunto = emailDAO.Asunto,
                ContenidoRespuesta = emailDAO.ContenidoRespuesta,
                Adjuntos = emailDAO.Adjuntos, // Asegúrate de pasar los adjuntos si es necesario

            };
            var resultado = _emailSenderService.ResponderCorreo(emailDAO);

            if (resultado.Success)
            {
                TempData["Success"] = $"Correo respondido correctamente a {emailDAO.Para}";
                return RedirectToAction("Index", "Home"); // O a donde lo necesites
            }
            else
            {
                ViewBag.Error = $"Error al enviar correo: {resultado.ErrorMessage}";
                return View(emailDAO);
            }
        }

    }
}
