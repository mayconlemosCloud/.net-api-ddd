using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // Configurar o DbContext
            services.AddDbContext<TaskManagementDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Registrar outros serviços da camada Infrastructure, se necessário

            return services;
        }
    }
}