using Application.DTO;
using Application.DTO.Usuarios;
using Domain.Entidades.Seguridad;
using Infrastructure.Persistence.Context;
//using Infrastructure.Persistence.Migrations;
using Microsoft.EntityFrameworkCore;
using Shared.Helper;


namespace Infrastructure.Repository
{
    public class UsuarioRepositoy
    {
        private readonly ApplicationDbContext _context;
        private readonly CryptoHelper _cryptoHelper;
        public UsuarioRepositoy(ApplicationDbContext appDBContext, CryptoHelper cryptoHelper)
        {
            _context = appDBContext;
            _cryptoHelper= cryptoHelper;
        }
        public async Task<Usuario?> ValidarUsuarioAsync(LoginDTO usuario)
        {
            try
            {
                var usuarioEncontrado = await _context.usuarios
                    .FirstOrDefaultAsync(u => u.CorreoElectronico == usuario.CorreoElectronico );

                if (usuarioEncontrado == null) return null;


                // Verificar si la contraseña ingresada coincide con el hash guardado
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

                await _context.usuarios.AddAsync(nuevoUsuario);
                await _context.SaveChangesAsync();

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
                var usuarioEncontrado = await _context.usuarios
                    .FirstOrDefaultAsync(u => u.CorreoElectronico == correo);

                return usuarioEncontrado;
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
                var usuarioExistente = await _context.usuarios.FindAsync(usuario.Id);
                if (usuarioExistente == null)
                    return (false, "Usuario no encontrado");

                usuarioExistente.PasswordSecret = _cryptoHelper.Encrypt(usuario.LlaveSecreta);

                _context.usuarios.Update(usuarioExistente);
                await _context.SaveChangesAsync();

                return (true, "Contraseña secreta actualizada correctamente");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

    }
}
