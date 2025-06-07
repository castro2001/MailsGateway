using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entidades.Seguridad;

namespace Infrastructure.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.HasKey(x => x.id);
            builder.Property(x => x.nombre).IsRequired().HasMaxLength(100);
            builder.Property(x => x.apellido).IsRequired().HasMaxLength(100);
            builder.Property(x => x.correoElectronico).HasMaxLength(100);
            builder.Property(x => x.clave).HasMaxLength(100);
            builder.Property(x => x.perfil).HasMaxLength(250);

        }
    }
}
