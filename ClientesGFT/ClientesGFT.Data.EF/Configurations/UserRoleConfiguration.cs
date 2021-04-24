using ClientesGFT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientesGFT.Data.EF.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> entity)
        {
            entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK__USUARIO_PERFIL");

            entity.ToTable("Usuario_Perfil");

            entity.Property(e => e.RoleId)
                .HasColumnName("IdPerfil");

            entity.Property(e => e.UserId)
                .HasColumnName("IdUsuario");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__USUARIO_PERFIL__PERFIL");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__USUARIO_PERFIL__USUARIO");
        }
    }
}
