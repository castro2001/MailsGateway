using MailGateway.Models;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Shared.Helper;
using Microsoft.AspNetCore.Authorization;
using Application.DTO.Mail;
namespace MailGateway.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailReaderService _emailReaderService;
        private readonly CryptoHelper _cryptoHelper;


        public HomeController(ILogger<HomeController> logger, IEmailReaderService emailReaderService, CryptoHelper cryptoHelper
            )
        {
            _logger = logger;
            _emailReaderService = emailReaderService;
            _cryptoHelper = cryptoHelper;


        }

        public IActionResult Index()
        {
            string errorMessage;
            var mensajes = _emailReaderService.LeerMensajesRecibidos(out errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                ViewBag.ErrorMessage = errorMessage;
            return View(mensajes);
        }

        public IActionResult Detalle( string uid )
        {
            string uidDesencriptado = _cryptoHelper.Decrypt(uid);
            uint uidValor = uint.Parse(uidDesencriptado);
            var correo = _emailReaderService.DetalleCorreo(uidValor);

            var fecha = FechaHelper.FormatearFecha(correo.Fecha);
            var fechaEmail = FechaHelper.FormatearComoEmail(correo.Fecha);
            var EmailBase = new EmailMessageBase()
            {
                Uid = _cryptoHelper.Encrypt(correo.Uid.ToString()), // UID del correo original, no encriptado
                Para = correo.Para,
                De = correo.De,
                Contenido = correo.Contenido,
                Asunto = correo.Asunto,
                FechaFormateada = fecha,
                Fecha = correo.Fecha,
                EnviadorPor = correo.EnviadoPor,
                FirmadoPor = correo.FirmadoPor,
                Seguridad = correo.Seguridad,
                Adjuntos = correo.Adjuntos

            };

            return View(EmailBase);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
