using ClientesGFT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            entity.ToTable("Clientes");

            entity.Ignore(e => e.IsEnableToModify);
            entity.Ignore(e => e.IsInternacional);

            entity.HasIndex(e => e.CPF)
                .HasName("UQ__Clientes__C1F89731CDBAAC7A")
                .IsUnique();

            entity.Property(e => e.CPF)
                .HasColumnName("CPF")
                .HasMaxLength(11)
                .IsUnicode(false);

            entity.Property(e => e.CreatedDate)
                .HasColumnName("DataCadastro")
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("DataAlteracao")
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.BirthDate)
                .HasColumnName("DataNasc")
                .HasColumnType("datetime");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Name)
                .HasColumnName("Nome")
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.RG)
                .HasColumnName("RG")
                .HasMaxLength(9)
                .IsUnicode(false);

            entity.Property(e => e.AdressId)
                .HasColumnName("IdEndereco");

            entity.Property(e => e.CurrentStatusId)
                .HasColumnName("IdStatusAtual");

            entity.HasOne(d => d.Adress)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.AdressId)
                .HasConstraintName("FK__CLIENTES__ENDERECO");

            entity.HasOne(d => d.CurrentStatus)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.CurrentStatusId)
                .HasConstraintName("FK__CLIENTES__STATUS_FLUXO");
        }
    }
}
