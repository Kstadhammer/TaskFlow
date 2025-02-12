using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Infrastructure;
using TaskFlow.Presentation;

var services = new ServiceCollection();

// Configure SQLite
var connectionString = "Data Source=taskflow.db";

// Register services
services.AddInfrastructure(connectionString);
services.AddPresentation();

// Build service provider
var serviceProvider = services.BuildServiceProvider();

// Create database and run migrations
using (var scope = serviceProvider.CreateScope())
{
    var context =
        scope.ServiceProvider.GetRequiredService<TaskFlow.Infrastructure.Data.Context.TaskFlowDbContext>();
    await context.Database.EnsureCreatedAsync();
}

// Run the application
var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
await mainMenu.ShowAsync();
