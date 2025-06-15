using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Seeders.Core;
using Infrastructure.Persistence.Seeders.Factory;


namespace Infrastructure.Persistence.Seeders
{
    public class UsuarioSeeder : ISeeder
    {
        public void Seed(ApplicationDbContext context)
        {
           if(!context.usuarios.Any())
            {
                var factory = UsuarioFactory.CrearUsuariosFake(10); // Crea 10 usuarios de ejemplo
                
                context.usuarios.AddRange(factory);
                context.SaveChanges();
            }


        }
    }
}
