using ClientesGFT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.ToTable("Perfils");

            entity.Ignore(e => e.DisplayName);

            entity.Property(e => e.Ativo).HasDefaultValueSql("((1))");

            entity.Property(e => e.Description)
                .HasColumnName("Nome")
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
        }
    }
}
