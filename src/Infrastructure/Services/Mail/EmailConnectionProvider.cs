using Application.DTO;
using Application.Interfaces;
using Application.Interfaces.Mail;
using Domain.Entidades.Mail;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
namespace Infrastructure.Services.Mail
{
    public class EmailConnectionProvider : IEmailConnectionProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ICredentialProvider _credentialProvider;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationStore _notificationStore; // Assuming you have a notification store implementation
        public EmailConnectionProvider(IConfiguration configuration, IHttpContextAccessor contextAccessor,
            ICredentialProvider credentialProvider, INotificationStore notificationStore)
        {
            _configuration = configuration;
            _credentialProvider=  credentialProvider;
            _contextAccessor = contextAccessor;
            _notificationStore = notificationStore;
        }


        public SmtpClient GetSmtpClient()
        {
           /* var correo = _contextAccessor.HttpContext?.User?.Claims
                                                       .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

           if (string.IsNullOrEmpty(correo))
                throw new Exception("Usuario no autenticado");*/
            //var (correoElectronico, PasswordSecret) = _credentialProvider.ObtenerCredencialesAsync(correo).Result;

            var smtp = new SmtpClient();
            // ⚠️ Solo para desarrollo:
            // smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            SecureSocketOptions _secureSocketOptions = SecureSocketOptions.StartTls; // Define the security options for the SMTP connection
            smtp.Connect(
                _configuration.GetSection("Email:Host").Value,
                Convert.ToInt32(_configuration.GetSection("Email:Port").Value),
                _secureSocketOptions
            );
            //// Usa credenciales dinámicas si están disponibles, sino usa las de config:
            //var username = _credentialProvider.Username ?? _configuration.GetSection("Email:Username").Value;
            //var password = _credentialProvider.Password ?? _configuration.GetSection("Email:Password").Value;


           // smtp.Authenticate(correoElectronico, PasswordSecret);

            //Authenticate the SMTP client using the credentials from configuration
            smtp.Authenticate(
                  _configuration.GetSection("Email:Username").Value,
                  _configuration.GetSection("Email:Password").Value);
            
              return smtp;
          }

          public ImapClient GetImapClient()
          {
            /*var correo = _contextAccessor.HttpContext?.User?.Claims
                                               .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

              if (string.IsNullOrEmpty(correo))
               throw new Exception("Usuario no autenticado");*/

            // var (correoElectronico, PasswordSecret) = _credentialProvider.ObtenerCredencialesAsync(correo).Result;

            try
            {
                var client = new ImapClient();
                client.Connect(
                    _configuration.GetSection("Email:Imap").Value,
                        Convert.ToInt32(_configuration.GetSection("Email:ImanPort").Value),
                     MailKit.Security.SecureSocketOptions.SslOnConnect
                        );
                //// Usa credenciales dinámicas si están disponibles, sino usa las de config:
                //var username = _credentialProvider.Username ?? _configuration.GetSection("Email:Username").Value;
                //var password = _credentialProvider.Password ?? _configuration.GetSection("Email:Password").Value;
                //Despues voy agregar validacion de usuario y contraseña por defecto
                //YO ya use mis credenciales de gmail y funcionan bien
                //Ahora solo falta que el usuario pueda agregar su contraseña de aplicación
                // client.Authenticate(correoElectronico, PasswordSecret);
                client.Authenticate(
                          _configuration.GetSection("Email:Username").Value,
                          _configuration.GetSection("Email:Password").Value
                      );
                return client;
            }
            catch (AuthenticationException)
            {
                throw new ApplicationException("No se pudo autenticar en el servidor de correo. Verifica tu usuario o contraseña.");
            }
            catch (SslHandshakeException)
            {
                throw new ApplicationException("No se pudo establecer una conexión segura con el servidor. Revisa el certificado SSL, y asegúrate de que ningún antivirus, proxy o firewall esté bloqueando la conexión.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al conectar con el servidor IMAP: {ex.Message}");
            }


        }
    }
}
