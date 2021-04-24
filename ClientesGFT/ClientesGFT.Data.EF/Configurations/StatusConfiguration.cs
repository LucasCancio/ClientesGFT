using ClientesGFT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> entity)
        {
            entity.ToTable("Status_Fluxo");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Description)
                .HasColumnName("Descricao")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
