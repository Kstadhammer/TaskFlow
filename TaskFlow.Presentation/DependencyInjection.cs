using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Presentation.UI;

namespace TaskFlow.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Register UI Components
        services.AddScoped<MainMenu>();
        services.AddScoped<ProjectMenu>();
        services.AddScoped<CustomerMenu>();
        services.AddScoped<ProjectManagerMenu>();
        services.AddScoped<ServiceMenu>();
        services.AddScoped<StatusMenu>();

        return services;
    }
}
