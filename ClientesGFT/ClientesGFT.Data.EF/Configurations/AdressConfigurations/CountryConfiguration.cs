using ClientesGFT.Domain.Entities.AdressEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations.AdressConfigurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {
            entity.ToTable("Paises");

            entity.HasIndex(e => e.Description)
                .HasName("UQ__Paises__A15FFF7DE1A4457E")
                .IsUnique();

            entity.HasIndex(e => e.Initials)
                .HasName("UQ__Paises__3199C5ED121356C0")
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Description)
                .HasColumnName("Pais")
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.Property(e => e.Initials)
                .HasColumnName("Sigla")
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);
        }
    }
}
