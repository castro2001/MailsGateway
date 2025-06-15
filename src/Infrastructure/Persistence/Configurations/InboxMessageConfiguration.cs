using Domain.Entidades.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Persistence.Configurations
{
    public class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("InboxMessage");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Uid).IsRequired().HasMaxLength(250);
            builder.Property(x => x.De).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Para).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Asunto).HasMaxLength(250);
            builder.Property(x => x.Contenido).IsRequired();
            builder.Property(x => x.Fecha).IsRequired();
            builder.Property(x => x.InReplyTo).HasMaxLength(100);
            builder.Property(x => x.EnviadoPor).HasMaxLength(250);
            builder.Property(x => x.FirmadoPor).HasMaxLength(250);
            builder.Property(x => x.Seguridad).HasMaxLength(100);
            builder.Property(x => x.EncryptedUid).HasMaxLength(100);

            // ✅ Relación con Usuario (Destinatario)
            builder.HasOne(x => x.Destinatario)
                   .WithMany(u => u.EmailRecibidos)
                   .HasForeignKey(x => x.DestinatarioID)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }

}
