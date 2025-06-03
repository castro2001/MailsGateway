using Application.Interfaces;
using Application.DTOS;
using MailGateway.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MailGateway.Controllers.Mail
{
    public class EnviadosController : Controller
    {
        private readonly IEmailReaderMessageService _emailReaderMessageService;
        public EnviadosController(IEmailReaderMessageService emailReaderMessageService)
        {
            _emailReaderMessageService = emailReaderMessageService;
        }
        public IActionResult Index()
        {
            var messages = _emailReaderMessageService.LeerMensajesEnviados();
            return View(messages);

        }
        public IActionResult Detalle(string uid)
        {
            string uidDesencriptado = CryptoHelper.Decrypt(uid);
            uint uidValor = uint.Parse(uidDesencriptado);
            var correo = _emailReaderMessageService.DetalleMensajesEnviados(uidValor);
            return View(correo);
        }
    }
}
