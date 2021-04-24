using ClientesGFT.Data.EF.Repositories;
using ClientesGFT.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientesGFT.Data.EF.Util
{
    public static class EFStartupExtension
    {
        public static IServiceCollection AddEF(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ClientesGFTContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ClientesGFT.Data.EF")));

            services.AddEFRepositories();

            return services;
        }

        private static IServiceCollection AddEFRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IClientRepository), typeof(ClientEFRepository));
            services.AddScoped(typeof(IFluxoRepository), typeof(FluxoEFRepository));
            services.AddScoped(typeof(IRoleRepository), typeof(RoleEFRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserEFRepository));
            services.AddScoped(typeof(IAdressRepository), typeof(AdressEFRepository));

            return services;
        }

    }
}
