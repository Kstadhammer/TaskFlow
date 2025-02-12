using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Infrastructure.Data.Context;
using TaskFlow.Infrastructure.Factories;
using TaskFlow.Infrastructure.Repositories;
using TaskFlow.Infrastructure.Services;

namespace TaskFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString
    )
    {
        // DbContext
        services.AddDbContext<TaskFlowDbContext>(options => options.UseSqlite(connectionString));

        // Repositories
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProjectManagerRepository, ProjectManagerRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();

        // Factories
        services.AddScoped<IProjectFactory, ProjectFactory>();
        services.AddScoped<ICustomerFactory, CustomerFactory>();
        services.AddScoped<IProjectManagerFactory, ProjectManagerFactory>();
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IStatusFactory, StatusFactory>();

        // Services
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProjectManagerService, ProjectManagerService>();
        services.AddScoped<IServiceEntityService, ServiceEntityService>();
        services.AddScoped<IStatusService, StatusService>();

        return services;
    }
}
