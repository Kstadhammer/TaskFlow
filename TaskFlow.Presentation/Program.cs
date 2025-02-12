using System.IO;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Infrastructure;
using TaskFlow.Presentation;
using TaskFlow.Presentation.UI;
using TaskFlow.Presentation.UI.Helpers;

// Show welcome screen
ConsoleUIHelper.ShowWelcomeScreen();

var services = new ServiceCollection();

// Configure SQLite database path
var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "taskflow.db");
var connectionString = $"Data Source={databasePath}";

// Register services
ConsoleUIHelper.ShowLoadingSpinner(
    "Initializing services",
    () =>
    {
        services.AddInfrastructure(connectionString);
        services.AddPresentation();
    }
);

// Build service provider
var serviceProvider = services.BuildServiceProvider();

// Create database and run migrations
ConsoleUIHelper.ShowLoadingSpinner(
    "Setting up database",
    async () =>
    {
        using var scope = serviceProvider.CreateScope();
        var context =
            scope.ServiceProvider.GetRequiredService<TaskFlow.Infrastructure.Data.Context.TaskFlowDbContext>();
        await context.Database.EnsureCreatedAsync();
    }
);

ConsoleUIHelper.DisplayInfo(
    "Welcome to TaskFlow! Use arrow keys to navigate and Enter to select options."
);

// Run the application
var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
await mainMenu.ShowAsync();
