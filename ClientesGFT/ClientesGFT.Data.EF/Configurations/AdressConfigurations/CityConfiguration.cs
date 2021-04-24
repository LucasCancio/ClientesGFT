using ClientesGFT.Domain.Entities.AdressEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations.AdressConfigurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> entity)
        {
            entity.ToTable("Cidades");

            entity.HasIndex(e => e.Description)
                .HasName("UQ__Cidades__15F7A8DA6F9206A9")
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Description)
                .HasColumnName("Cidade")
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.StateId)
                .HasColumnName("IdEstado");

            entity.HasOne(d => d.State)
                .WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CIDADES__ESTADO");
        }
    }
}
