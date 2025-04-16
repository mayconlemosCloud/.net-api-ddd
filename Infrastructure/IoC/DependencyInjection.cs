using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Application.Mappings;
using AutoMapper;
using Domain.Repositories;
using Domain.Entities;
using Infrastructure.Repositories;
using Application.Services.Interfaces;
using Application.Services;

namespace Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // Configurar o DbContext
            services.AddDbContext<TaskManagementDbContext>(options =>
                options.UseNpgsql(connectionString));


            services.AddAutoMapper(typeof(AutoMapperProfile));


            services.AddScoped<IBaseRepository<Project>, ProjectRepository>();
            services.AddScoped<IBaseRepository<TaskEntity>, TaskRepository>();
            services.AddScoped<IBaseRepository<User>, UserRepository>();



            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();


            return services;
        }
    }
}