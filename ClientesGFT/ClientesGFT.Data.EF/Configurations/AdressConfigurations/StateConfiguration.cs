using ClientesGFT.Domain.Entities.AdressEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations.AdressConfigurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> entity)
        {
            entity.ToTable("Estados");

            entity.HasIndex(e => e.Description)
                .HasName("UQ__Estados__36DF552F9175DA7F")
                .IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Description)
                .HasColumnName("Estado")
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.CountryId)
                .HasColumnName("IdPais");

            entity.HasOne(d => d.Country)
                .WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ESTADOS__PAIS");
        }
    }
}
