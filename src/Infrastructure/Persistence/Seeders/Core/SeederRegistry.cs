using Infrastructure.Persistence.Context;


namespace Infrastructure.Persistence.Seeders.Core
{
    public class SeederRegistry
    {
        public static void RegisterAll() {
            DatabaseSeeder.AddSeeder(new UsuarioSeeder());
                

        }
    }
}
