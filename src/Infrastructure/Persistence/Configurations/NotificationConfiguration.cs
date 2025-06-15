using Domain.Entidades.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            builder.HasKey(n => n.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(n => n.Titulo).IsRequired() .HasMaxLength(200);

            builder.Property(n => n.Mensaje)
                   .IsRequired();

            builder.Property(n => n.Icono).HasMaxLength(100);

            builder.Property(n => n.Tipo) .HasMaxLength(50) .IsRequired();

            builder.Property(n => n.Url).HasMaxLength(500);

            builder.Property(n => n.Leido) .HasDefaultValue(false);

            builder.Property(n => n.Fecha). HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
                   
            builder.HasOne(n => n.Usuario)
                   .WithMany(u => u.Notificaciones)
                   .HasForeignKey(n => n.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
