using ClientesGFT.Data.Repositories;
using ClientesGFT.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ClientesGFT.Data.DAL.Util
{
    public static class SQLStartupExtension
    {
        public static IServiceCollection AddDAL(this IServiceCollection services)
        {
            services.AddScoped(typeof(IClientRepository), typeof(ClientSQLRepository));
            services.AddScoped(typeof(IFluxoRepository), typeof(FluxoSQLRepository));
            services.AddScoped(typeof(IRoleRepository), typeof(RoleSQLRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserSQLRepository));
            services.AddScoped(typeof(IAdressRepository), typeof(AdressSQLRepository));


            return services;
        }
    }
}
