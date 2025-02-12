using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Presentation.UI.Helpers;

namespace TaskFlow.Presentation.UI;

public class ServiceMenu
{
    private readonly IServiceEntityService _serviceService;

    public ServiceMenu(IServiceEntityService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            var options = new Dictionary<string, string>
            {
                { "1", "View All Services" },
                { "2", "Add New Service" },
                { "3", "Edit Service" },
                { "4", "View Service Details" },
                { "0", "Back to Main Menu" },
            };

            var choice = MenuHelper.ShowMenu("Service Management", options);

            switch (choice)
            {
                case "1":
                    await ViewAllServices();
                    break;
                case "2":
                    await AddNewService();
                    break;
                case "3":
                    await EditService();
                    break;
                case "4":
                    await ViewServiceDetails();
                    break;
                case "0":
                    return;
            }
        }
    }

    private async Task ViewAllServices()
    {
        var result = await _serviceService.GetAllAsync();
        if (result.Success)
        {
            ConsoleHelper.DisplayHeader("All Services");
            Console.WriteLine("ID\tName\tHourly Rate\tActive");
            Console.WriteLine("----------------------------------------");
            foreach (var service in result.Data)
            {
                Console.WriteLine(
                    $"{service.Id}\t{service.Name}\t{service.HourlyRate:C}\t{(service.IsActive ? "Yes" : "No")}"
                );
            }
            ConsoleHelper.PressAnyKey();
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task AddNewService()
    {
        ConsoleHelper.DisplayHeader("Add New Service");

        var name = ConsoleHelper.GetRequiredInput("Name");
        var description = ConsoleHelper.GetRequiredInput("Description");
        var hourlyRate = ConsoleHelper.GetDecimal("Hourly Rate");

        var request = new CreateServiceRequest
        {
            Name = name,
            Description = description,
            HourlyRate = hourlyRate,
            IsActive = true,
        };

        var result = await _serviceService.CreateAsync(request);
        if (result.Success)
        {
            ConsoleHelper.DisplaySuccess("Service created successfully");
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task EditService()
    {
        ConsoleHelper.DisplayHeader("Edit Service");

        var id = ConsoleHelper.GetId("Enter Service ID");

        var serviceResult = await _serviceService.GetByIdAsync(id);
        if (!serviceResult.Success || serviceResult.Data is null)
        {
            ConsoleHelper.DisplayError(serviceResult.Message);
            return;
        }

        var service = serviceResult.Data;
        Console.WriteLine($"\nEditing Service: {service.Name}");

        var name = ConsoleHelper.GetRequiredInput("New Name");
        var description = ConsoleHelper.GetRequiredInput("New Description");
        var hourlyRate = service.HourlyRate;
        Console.Write($"New Hourly Rate ({service.HourlyRate:C}): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal newRate))
        {
            hourlyRate = newRate;
        }

        var request = new UpdateServiceRequest
        {
            Id = id,
            Name = name,
            Description = description,
            HourlyRate = hourlyRate,
            IsActive = service.IsActive,
        };

        var result = await _serviceService.UpdateAsync(request);
        if (result.Success)
        {
            ConsoleHelper.DisplaySuccess("Service updated successfully");
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task ViewServiceDetails()
    {
        ConsoleHelper.DisplayHeader("Service Details");

        var id = ConsoleHelper.GetId("Enter Service ID");

        var result = await _serviceService.GetByIdAsync(id);
        if (!result.Success || result.Data is null)
        {
            ConsoleHelper.DisplayError(result.Message);
            return;
        }

        var service = result.Data;
        Console.WriteLine($"\nID: {service.Id}");
        Console.WriteLine($"Name: {service.Name}");
        Console.WriteLine($"Description: {service.Description}");
        Console.WriteLine($"Hourly Rate: {service.HourlyRate:C}");
        Console.WriteLine($"Active: {(service.IsActive ? "Yes" : "No")}");

        if (service.Projects?.Any() == true)
        {
            Console.WriteLine("\nProjects using this service:");
            foreach (var project in service.Projects)
            {
                Console.WriteLine($"- {project.ProjectNumber}: {project.Name}");
            }
        }
        else
        {
            Console.WriteLine("\nNo projects are currently using this service");
        }
        ConsoleHelper.PressAnyKey();
    }
}
