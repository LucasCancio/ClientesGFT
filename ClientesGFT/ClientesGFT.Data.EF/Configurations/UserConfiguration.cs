using ClientesGFT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Usuarios");

            entity.Ignore(e => e.Roles);
            entity.Ignore(e => e.IsEnableToModify);

            entity.HasIndex(e => e.Login)
                .HasName("UQ__Usuarios__5E55825BE0CCFE30")
                .IsUnique();

            entity.Property(e => e.Ativo).HasDefaultValueSql("((1))");

            entity.Property(e => e.CreatedDate)
                .HasColumnName("DataCadastro")
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Login)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .HasColumnName("Nome")
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Password)
                .HasColumnName("Senha")
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
        }
    }
}
