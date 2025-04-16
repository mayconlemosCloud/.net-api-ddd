using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Application.Mappings;
using AutoMapper;
using Domain.Repositories;
using Domain.Entities;
using Infrastructure.Repositories;

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
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // Register repository interfaces
            services.AddScoped<IBaseRepository<Project>, ProjectRepository>();
            services.AddScoped<IBaseRepository<TaskEntity>, TaskRepository>();

            return services;
        }
    }
}