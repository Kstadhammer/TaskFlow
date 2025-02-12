using System.IO;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Infrastructure;
using TaskFlow.Presentation;
using TaskFlow.Presentation.UI;

var services = new ServiceCollection();

// Configure SQLite database path
var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "taskflow.db");
var connectionString = $"Data Source={databasePath}";

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
