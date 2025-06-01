using Application.Interfaces;
using Application.DTOS;
using MailGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace MailGateway.Controllers
{
    public class RedactarController : Controller
    {
        private readonly IEmailSenderService _emailSenderService;
        public RedactarController(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService; 
        }
        public IActionResult Redactar()
        {
     
            return View();
        }

        [HttpPost]
        public IActionResult Redactar(EmailDTO emailDAO)
        {
          
            if(string.IsNullOrWhiteSpace(emailDAO.Para)) { 
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

            EmailDTO email = new EmailDTO() { 
                Para = emailDAO.Para,
                Asunto = emailDAO.Asunto,
                Contenido = emailDAO.Contenido
            };

            var resultado = _emailSenderService.SendEmail(email);

            if (resultado.Success)
            {
                // Guardar correo enviado
                ViewBag.Success = "Correo enviado correctamente. " ;
                return View(new EmailDTO());
            }
            else {
                ViewBag.Error = $"Error al enviar correo: {resultado.ErrorMessage}";
            }
            return View();

        }
    }
}
