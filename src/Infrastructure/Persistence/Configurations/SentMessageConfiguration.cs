using Domain.Entidades.Mail;
using Domain.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SentMessageConfiguration : IEntityTypeConfiguration<SentMessage>
    {
        public void Configure(EntityTypeBuilder<SentMessage> builder)
        {
            builder.ToTable("SentMessage");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.MessageId).HasMaxLength(100);
            builder.Property(x => x.Para).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Asunto).HasMaxLength(250);
            builder.Property(x => x.InReplyTo).HasMaxLength(100);
            builder.Property(x => x.FechaEnvio).IsRequired();
            // 👇 Esta parte estaba faltando (relación con Usuario)
            builder.HasOne(x => x.Remitente)
                   .WithMany(u => u.MensajesEnviados) // asegúrate que esto esté en Usuario
                   .HasForeignKey(x => x.RemitenteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
