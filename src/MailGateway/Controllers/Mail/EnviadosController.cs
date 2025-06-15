using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helper;

namespace MailGateway.Controllers.Mail
{
    [Authorize]
    public class EnviadosController : Controller
    {
        private readonly IEmailReaderMessageService _emailReaderMessageService;
        private readonly CryptoHelper _cryptoHelper;

        public EnviadosController(IEmailReaderMessageService emailReaderMessageService, CryptoHelper cryptoHelper)
        {
            _emailReaderMessageService = emailReaderMessageService;
            _cryptoHelper= cryptoHelper;
        }
        public IActionResult Index()
        {
            var messages = _emailReaderMessageService.LeerMensajesEnviados();
            // Encriptar UID aquí
            foreach (var message in messages)
            {
                message.EncryptedUid = _cryptoHelper.Encrypt(message.Uid.ToString());
            }
            return View(messages);

        }
        public IActionResult Detalle(string uid)
        {
            string uidDesencriptado = _cryptoHelper.Decrypt(uid);
            uint uidValor = uint.Parse(uidDesencriptado);
            var correo = _emailReaderMessageService.DetalleMensajesEnviados(uidValor);
            return View(correo);
        }
    }
}
