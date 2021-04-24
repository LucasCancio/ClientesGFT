using ClientesGFT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations
{
    public class FluxoConfiguration : IEntityTypeConfiguration<Fluxo>
    {
        public void Configure(EntityTypeBuilder<Fluxo> entity)
        {
            entity.ToTable("Fluxo_Aprovacao");

            entity.Property(e => e.CreateDate)
                .HasColumnName("DataCriacao")
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ClientId)
                .HasColumnName("IdCliente");

            entity.Property(e => e.StatusId)
                .HasColumnName("IdStatus");

            entity.Property(e => e.UserId)
                .HasColumnName("IdUsuario");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.Fluxos)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FLUXO_APROVACAO__CLIENTE");

            entity.HasOne(d => d.Status)
                .WithMany(p => p.Fluxos)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FLUXO_APROVACAO__STATUS_ANTERIOR__STATUS_FLUXO");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Fluxos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FLUXO_APROVACAO__USUARIO");
        }
    }
}
