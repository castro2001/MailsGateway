using Infrastructure.Persistence.Seeders.Core;


namespace Infrastructure.Persistence.Context
{
    public static class DatabaseSeeder 
    {
        private static readonly List<ISeeder> _seeders = new() ;

        public static void AddSeeder(ISeeder seeder)
        {
            _seeders.Add(seeder);
        }
        public static void SeedAll( ApplicationDbContext context){
            foreach (var seeder in _seeders)
            {
                seeder.Seed(context);
            }
        }
    }
}
