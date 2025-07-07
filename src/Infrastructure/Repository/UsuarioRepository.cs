using Application.DTO;
using Application.DTO.Usuarios;
using Application.Interfaces;
using Domain.Entidades.Seguridad;
using Shared.Helper;


namespace Infrastructure.Repository
{
    public class UsuarioRepository
    {
        //private readonly ApplicationDbContext _context;
        private readonly INotificationStore _notificationStore;
        private readonly CryptoHelper _cryptoHelper;
        public UsuarioRepository( CryptoHelper cryptoHelper, INotificationStore notificationStore)
        {
            //_context = appDBContext;
            _notificationStore = notificationStore;
            _cryptoHelper = cryptoHelper;
        }
        public async Task<Usuario?> ValidarUsuarioAsync(LoginDTO usuario)
        {
            try
            {
                /*
                var usuarioEncontrado = await _context.usuarios
                    .FirstOrDefaultAsync(u => u.CorreoElectronico == usuario.CorreoElectronico);

                if (usuarioEncontrado == null) return null;


                // Verificar si la contraseña ingresada coincide con el hash guardado
                bool claveValida = SecurityHelper.VerifyPassword(usuario.Clave, usuarioEncontrado.Clave);
                return claveValida ? usuarioEncontrado : null;*/
                // Buscar usuario en el "almacén" de memoria
                var usuarios = _notificationStore.Obtener(obj => obj as Usuario);
                var usuarioEncontrado = usuarios
                    .FirstOrDefault(u => u != null && u.CorreoElectronico == usuario.CorreoElectronico);

                if (usuarioEncontrado == null)
                    return null;

                // Verificar la clave
                bool claveValida = SecurityHelper.VerifyPassword(usuario.Clave, usuarioEncontrado.Clave);
                return claveValida ? usuarioEncontrado : null;


            }
            catch (Exception ex)
            {
                // Puedes registrar el error con un logger si tienes uno configurado
                return null;
            }
        }
        public async Task<(bool Exito, string? Message)> crearUsuario(RegistroDTO usuario)
        {
            try
            {
                var nuevoUsuario = new Usuario
                {
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    CorreoElectronico = usuario.CorreoElectronico,
                    Clave = SecurityHelper.HashPassword(usuario.Clave),
                    Perfil = usuario.Perfil,
                    PasswordSecret = _cryptoHelper.Encrypt(usuario.LlaveSecreta),
                };

                _notificationStore.Agregar(nuevoUsuario);
                //  await _context.usuarios.AddAsync(nuevoUsuario);
                //  await _context.SaveChangesAsync();

                return (true, "Registro Guardado Correctamente");
            }
            catch (Exception ex)
            {
                // Puedes registrar el error con un logger si tienes uno configurado
                return (false, ex.Message);
            }
        }

        public async Task<Usuario?> obtenerUsuarioCorreo(string correo)
        {
            try
            {
                /*var usuarioEncontrado = await _context.usuarios
                    .FirstOrDefaultAsync(u => u.CorreoElectronico == correo);*/
                var usuarios = _notificationStore.Obtener(obj => obj as Usuario);
                return usuarios.FirstOrDefault(u => u != null && u.CorreoElectronico == correo);

            }
            catch (Exception ex)
            {
                // Aquí podrías usar un logger, por ejemplo: _logger.LogError(ex, "Error al buscar usuario");
                return null;
            }
        }

        public async Task<(bool Exito, string? Message)> ActualizarPasswordSecret(UsuarioDTO usuario)
        {
            try
            {
               /* var usuarioExistente = await _context.usuarios.FindAsync(usuario.Id);
                if (usuarioExistente == null)
                    return (false, "Usuario no encontrado");

                usuarioExistente.PasswordSecret = _cryptoHelper.Encrypt(usuario.LlaveSecreta);

                _context.usuarios.Update(*);
                await _context.SaveChangesAsync*/

                return (true, "Contraseña secreta actualizada correctamente");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

    }
}
