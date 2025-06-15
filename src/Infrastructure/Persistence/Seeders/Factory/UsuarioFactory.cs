using Domain.Entidades.Seguridad;
using Bogus;
using Shared.Helper;
using Org.BouncyCastle.Crypto.Generators;


namespace Infrastructure.Persistence.Seeders.Factory
{
    public static class UsuarioFactory
    {
        public static Usuario CreateUsuario(string nombre, string apellido, string correoElectronico, string clave, string perfil)
        {
            return new Usuario
            {
                 // Assuming id is a Guid
                Nombre = nombre,
                Apellido = apellido,
                CorreoElectronico = correoElectronico,
                Clave = clave,
                Perfil = perfil
            };
        }
        public static List<Usuario> CrearUsuariosFake(int cantidad)
        {
            var usuarios = new List<Usuario>();
            var faker = new Bogus.Faker("es");

            for (int i = 0; i < cantidad; i++)
            {
                var usuario = new Usuario
                {
                    Nombre = faker.Name.FirstName(),
                    Apellido = faker.Name.LastName(),
                    CorreoElectronico = faker.Internet.Email(),
                    Clave = faker.Internet.Password(10, true, "[A-Z]", "1!"),
                    FechaCreacion = faker.Date.Between(new DateTime(1980, 1, 1), new DateTime(2025, 12, 31)),
                    Perfil = "Usuario"
                };

                usuarios.Add(usuario);
            }

            return usuarios;
        }

    }
}
