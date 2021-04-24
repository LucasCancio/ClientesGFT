using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ClientesGFT.Domain.Entities.AdressEntities;

namespace ClientesGFT.Data.EF.Configurations.AdressConfigurations
{
    public class AdressConfiguration : IEntityTypeConfiguration<Adress>
    {
        public void Configure(EntityTypeBuilder<Adress> entity)
        {
            entity.ToTable("Enderecos");

            entity.Property(e => e.District)
                .HasColumnName("Bairro")
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.Property(e => e.Complement)
                .HasColumnName("Complemento")
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Street)
                .HasColumnName("Rua")
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Number)
                .HasColumnName("Numero");

            entity.Property(e => e.CityId)
                .HasColumnName("IdCidade");

            entity.HasOne(d => d.City)
                .WithMany(p => p.Adresses)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__ENDERECOS__CIDADE");
        }
    }
}
