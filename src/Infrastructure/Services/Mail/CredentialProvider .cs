using Application.DTO;
using Application.Interfaces.Mail;
using Infrastructure.Repository;
using Shared.Helper;

namespace Domain.Entidades.Mail
{
    public class CredentialProvider : ICredentialProvider
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly CryptoHelper _cryptoHelper;
        public CredentialProvider(UsuarioRepository usuarioRepository, CryptoHelper cryptoHelper)
        {
            _cryptoHelper = cryptoHelper;
            _usuarioRepository = usuarioRepository;
        }
        public async Task<(string Correo, string ClaveAplicacion)> ObtenerCredencialesAsync(string correo)
        {
            var usuario = await _usuarioRepository.obtenerUsuarioCorreo(correo);
            if (usuario == null || string.IsNullOrEmpty(usuario.PasswordSecret))
                throw new Exception("Usuario no encontrado o sin clave de aplicación configurada");

            var clave = _cryptoHelper.Decrypt(usuario.PasswordSecret);
            return (usuario.CorreoElectronico, clave);
        }
    }
}
