using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Presentation.UI.Helpers;

namespace TaskFlow.Presentation.UI;

public class StatusMenu
{
    private readonly IStatusService _statusService;

    public StatusMenu(IStatusService statusService)
    {
        _statusService = statusService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            var options = new Dictionary<string, string>
            {
                { "1", "View All Statuses" },
                { "2", "Add New Status" },
                { "3", "Edit Status" },
                { "4", "View Status Details" },
                { "0", "Back to Main Menu" },
            };

            var choice = MenuHelper.ShowMenu("Status Management", options);

            switch (choice)
            {
                case "1":
                    await ViewAllStatuses();
                    break;
                case "2":
                    await AddNewStatus();
                    break;
                case "3":
                    await EditStatus();
                    break;
                case "4":
                    await ViewStatusDetails();
                    break;
                case "0":
                    return;
            }
        }
    }

    private async Task ViewAllStatuses()
    {
        var result = await _statusService.GetAllAsync();
        if (result.Success)
        {
            ConsoleHelper.DisplayHeader("All Statuses");
            Console.WriteLine("ID\tName\tDescription");
            Console.WriteLine("----------------------------------------");
            foreach (var status in result.Data)
            {
                Console.WriteLine($"{status.Id}\t{status.Name}\t{status.Description}");
            }
            ConsoleHelper.PressAnyKey();
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task AddNewStatus()
    {
        ConsoleHelper.DisplayHeader("Add New Status");

        var name = ConsoleHelper.GetRequiredInput("Status Name");
        var description = ConsoleHelper.GetRequiredInput("Description");

        var request = new CreateStatusRequest { Name = name, Description = description };

        var result = await _statusService.CreateAsync(request);
        if (result.Success)
        {
            ConsoleHelper.DisplaySuccess("Status created successfully");
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task EditStatus()
    {
        ConsoleHelper.DisplayHeader("Edit Status");

        var id = ConsoleHelper.GetId("Enter Status ID");

        var statusResult = await _statusService.GetByIdAsync(id);
        if (!statusResult.Success)
        {
            ConsoleHelper.DisplayError(statusResult.Message);
            return;
        }

        var status = statusResult.Data;
        Console.WriteLine($"\nEditing Status: {status.Name}");

        var name = ConsoleHelper.GetOptionalInput("New Name", status.Name);
        var description = ConsoleHelper.GetOptionalInput("New Description", status.Description);

        var request = new UpdateStatusRequest
        {
            Id = id,
            Name = name,
            Description = description,
        };

        var result = await _statusService.UpdateAsync(request);
        if (result.Success)
        {
            ConsoleHelper.DisplaySuccess("Status updated successfully");
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task ViewStatusDetails()
    {
        ConsoleHelper.DisplayHeader("Status Details");

        var id = ConsoleHelper.GetId("Enter Status ID");

        var result = await _statusService.GetByIdAsync(id);
        if (result.Success)
        {
            var status = result.Data;
            Console.WriteLine($"\nID: {status.Id}");
            Console.WriteLine($"Name: {status.Name}");
            Console.WriteLine($"Description: {status.Description}");

            if (status.Projects?.Any() == true)
            {
                Console.WriteLine("\nProjects with this status:");
                foreach (var project in status.Projects)
                {
                    Console.WriteLine($"- {project.ProjectNumber}: {project.Name}");
                }
            }
            else
            {
                Console.WriteLine("\nNo projects are currently using this status");
            }
            ConsoleHelper.PressAnyKey();
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }
}
