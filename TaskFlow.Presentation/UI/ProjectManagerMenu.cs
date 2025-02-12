using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Presentation.UI.Helpers;

namespace TaskFlow.Presentation.UI;

public class ProjectManagerMenu
{
    private readonly IProjectManagerService _projectManagerService;

    public ProjectManagerMenu(IProjectManagerService projectManagerService)
    {
        _projectManagerService = projectManagerService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            var options = new Dictionary<string, string>
            {
                { "1", "View All Project Managers" },
                { "2", "Add New Project Manager" },
                { "3", "Edit Project Manager" },
                { "4", "View Project Manager Details" },
                { "0", "Back to Main Menu" },
            };

            var choice = MenuHelper.ShowMenu("Project Manager Management", options);

            switch (choice)
            {
                case "1":
                    await ViewAllProjectManagers();
                    break;
                case "2":
                    await AddNewProjectManager();
                    break;
                case "3":
                    await EditProjectManager();
                    break;
                case "4":
                    await ViewProjectManagerDetails();
                    break;
                case "0":
                    return;
            }
        }
    }

    private async Task ViewAllProjectManagers()
    {
        var result = await _projectManagerService.GetAllAsync();
        if (result.Success)
        {
            ConsoleHelper.DisplayHeader("All Project Managers");
            Console.WriteLine("ID\tName\tEmail\tDepartment\tTitle");
            Console.WriteLine("----------------------------------------");
            foreach (var manager in result.Data)
            {
                Console.WriteLine(
                    $"{manager.Id}\t{manager.Name}\t{manager.Email}\t{manager.Department}\t{manager.Title}"
                );
            }
            ConsoleHelper.PressAnyKey();
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task AddNewProjectManager()
    {
        ConsoleHelper.DisplayHeader("Add New Project Manager");

        var name = ConsoleHelper.GetRequiredInput("Name");
        var email = ConsoleHelper.GetRequiredInput("Email");
        var phoneNumber = ConsoleHelper.GetRequiredInput("Phone Number");
        var department = ConsoleHelper.GetRequiredInput("Department");
        var title = ConsoleHelper.GetRequiredInput("Title");

        var request = new CreateProjectManagerRequest
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            Department = department,
            Title = title,
        };

        var result = await _projectManagerService.CreateAsync(request);
        if (result.Success)
        {
            ConsoleHelper.DisplaySuccess("Project Manager created successfully");
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task EditProjectManager()
    {
        ConsoleHelper.DisplayHeader("Edit Project Manager");

        var id = ConsoleHelper.GetId("Enter Project Manager ID");

        var managerResult = await _projectManagerService.GetByIdAsync(id);
        if (!managerResult.Success || managerResult.Data is null)
        {
            ConsoleHelper.DisplayError(managerResult.Message);
            return;
        }

        var manager = managerResult.Data;
        Console.WriteLine($"\nEditing Project Manager: {manager.Name}");

        var name = ConsoleHelper.GetRequiredInput("New Name");
        var email = ConsoleHelper.GetRequiredInput("New Email");
        var phoneNumber = ConsoleHelper.GetRequiredInput("New Phone Number");
        var department = ConsoleHelper.GetRequiredInput("New Department");
        var title = ConsoleHelper.GetRequiredInput("New Title");

        var request = new UpdateProjectManagerRequest
        {
            Id = id,
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            Department = department,
            Title = title,
        };

        var result = await _projectManagerService.UpdateAsync(request);
        if (result.Success)
        {
            ConsoleHelper.DisplaySuccess("Project Manager updated successfully");
        }
        else
        {
            ConsoleHelper.DisplayError(result.Message);
        }
    }

    private async Task ViewProjectManagerDetails()
    {
        ConsoleHelper.DisplayHeader("Project Manager Details");

        var id = ConsoleHelper.GetId("Enter Project Manager ID");

        var result = await _projectManagerService.GetByIdAsync(id);
        if (!result.Success || result.Data is null)
        {
            ConsoleHelper.DisplayError(result.Message);
            return;
        }

        var manager = result.Data;
        Console.WriteLine($"\nID: {manager.Id}");
        Console.WriteLine($"Name: {manager.Name}");
        Console.WriteLine($"Email: {manager.Email}");
        Console.WriteLine($"Phone Number: {manager.PhoneNumber}");
        Console.WriteLine($"Department: {manager.Department}");
        Console.WriteLine($"Title: {manager.Title}");

        if (manager.Projects?.Any() == true)
        {
            Console.WriteLine("\nManaged Projects:");
            foreach (var project in manager.Projects)
            {
                Console.WriteLine($"- {project.ProjectNumber}: {project.Name}");
            }
        }
        else
        {
            Console.WriteLine("\nNo projects currently managed");
        }
        ConsoleHelper.PressAnyKey();
    }
}
