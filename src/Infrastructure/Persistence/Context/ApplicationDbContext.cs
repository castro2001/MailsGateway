
using Domain.Entidades.Mail;
using Domain.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
     
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<InboxMessage> InboxMessage { get; set; }
        public DbSet<SentMessage> SentMessage { get; set; }
        public DbSet<Notification> Notification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplica automáticamente todas las configuraciones IEntityTypeConfiguration<T>
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
