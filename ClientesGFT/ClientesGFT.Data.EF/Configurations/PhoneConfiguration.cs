using ClientesGFT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations
{
    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> entity)
        {
            entity.ToTable("Clientes_Telefones");

            entity.Property(e => e.Number)
                .HasColumnName("Telefone")
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.ClientId)
                .HasColumnName("IdCliente");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.Phones)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__CLIENTES_TELEFONES__CLIENTE");
        }
    }
}
