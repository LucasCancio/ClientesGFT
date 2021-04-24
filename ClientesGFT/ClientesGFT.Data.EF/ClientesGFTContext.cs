using ClientesGFT.Data.EF.Configurations;
using ClientesGFT.Data.EF.Configurations.AdressConfigurations;
using ClientesGFT.Domain.Entities;
using ClientesGFT.Domain.Entities.AdressEntities;
using Microsoft.EntityFrameworkCore;

namespace ClientesGFT.Data.EF
{
    public partial class ClientesGFTContext : DbContext
    {
        public ClientesGFTContext()
        {
        }

        public ClientesGFTContext(DbContextOptions<ClientesGFTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Adress> Adresses { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Country> Countries { get; set; }


        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        
        public virtual DbSet<Fluxo> Fluxos { get; set; }
        
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneConfiguration());
            modelBuilder.ApplyConfiguration(new FluxoConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());


            modelBuilder.ApplyConfiguration(new AdressConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new StateConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
