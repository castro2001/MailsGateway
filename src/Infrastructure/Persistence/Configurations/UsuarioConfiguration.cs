using Domain.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CorreoElectronico).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Clave).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Perfil).IsRequired().HasMaxLength(250);
            builder.Property(x => x.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.FechaModificacion).HasColumnType("datetime");
            builder.Property(x => x.PasswordSecret).HasMaxLength(250);
           
            // Relaciones
            builder.HasMany(x => x.MensajesEnviados)
                   .WithOne(m => m.Remitente)
                   .HasForeignKey(m => m.RemitenteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.EmailRecibidos)
                   .WithOne(m => m.Destinatario)
                   .HasForeignKey(m => m.DestinatarioID)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Notificaciones)
                   .WithOne(n => n.Usuario)
                   .HasForeignKey(n => n.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
