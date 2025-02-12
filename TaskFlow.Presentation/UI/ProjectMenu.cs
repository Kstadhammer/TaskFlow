using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Presentation.UI.Helpers;

namespace TaskFlow.Presentation.UI
{
    public class ProjectMenu
    {
        private readonly IProjectService _projectService;

        public ProjectMenu(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                var options = new Dictionary<string, string>
                {
                    { "1", "View All Projects" },
                    { "2", "Add New Project" },
                    { "3", "Edit Project" },
                    { "4", "View Project Details" },
                    { "0", "Back to Main Menu" },
                };

                var choice = MenuHelper.ShowMenu("Project Management", options);

                switch (choice)
                {
                    case "1":
                        await ViewAllProjects();
                        break;
                    case "2":
                        await AddNewProject();
                        break;
                    case "3":
                        await EditProject();
                        break;
                    case "4":
                        await ViewProjectDetails();
                        break;
                    case "0":
                        return;
                }
            }
        }

        private async Task ViewAllProjects()
        {
            var result = await _projectService.GetAllAsync();
            if (result.Success)
            {
                ConsoleHelper.DisplayHeader("All Projects");
                Console.WriteLine("ID\tProject Number\tName\tStatus");
                Console.WriteLine("----------------------------------------");
                foreach (var project in result.Data)
                {
                    Console.WriteLine(
                        $"{project.Id}\t{project.ProjectNumber}\t{project.Name}\t{project.Status.Name}"
                    );
                }
                ConsoleHelper.PressAnyKey();
            }
            else
            {
                ConsoleHelper.DisplayError(result.Message);
            }
        }

        private async Task AddNewProject()
        {
            try
            {
                ConsoleUIHelper.DrawBox("Add New Project");

                // Get project name with validation
                string name;
                do
                {
                    name = ConsoleUIHelper.GetInput("Project Name", ConsoleColor.Cyan).Trim();
                    if (string.IsNullOrEmpty(name))
                    {
                        ConsoleUIHelper.DisplayError(
                            "Project name cannot be empty. Please try again."
                        );
                    }
                } while (string.IsNullOrEmpty(name));

                // Get start date with validation
                DateTime startDate;
                do
                {
                    var startDateStr = ConsoleUIHelper.GetInput(
                        "Start Date (yyyy-MM-dd)",
                        ConsoleColor.Cyan
                    );
                    if (!DateTime.TryParse(startDateStr, out startDate))
                    {
                        ConsoleUIHelper.DisplayError(
                            "Invalid date format. Please use yyyy-MM-dd format."
                        );
                        continue;
                    }
                    break;
                } while (true);

                // Get end date with validation
                DateTime endDate;
                do
                {
                    var endDateStr = ConsoleUIHelper.GetInput(
                        "End Date (yyyy-MM-dd)",
                        ConsoleColor.Cyan
                    );
                    if (!DateTime.TryParse(endDateStr, out endDate))
                    {
                        ConsoleUIHelper.DisplayError(
                            "Invalid date format. Please use yyyy-MM-dd format."
                        );
                        continue;
                    }
                    if (endDate < startDate)
                    {
                        ConsoleUIHelper.DisplayError("End date must be after start date.");
                        continue;
                    }
                    break;
                } while (true);

                // Get total price with validation
                decimal totalPrice;
                do
                {
                    var totalPriceStr = ConsoleUIHelper.GetInput("Total Price", ConsoleColor.Cyan);
                    if (!decimal.TryParse(totalPriceStr, out totalPrice))
                    {
                        ConsoleUIHelper.DisplayError(
                            "Invalid price format. Please enter a valid number."
                        );
                        continue;
                    }
                    if (totalPrice < 0)
                    {
                        ConsoleUIHelper.DisplayError("Price cannot be negative.");
                        continue;
                    }
                    break;
                } while (true);

                // Get customer ID with validation
                int customerId;
                do
                {
                    var customerIdStr = ConsoleUIHelper.GetInput("Customer ID", ConsoleColor.Cyan);
                    if (!int.TryParse(customerIdStr, out customerId))
                    {
                        ConsoleUIHelper.DisplayError(
                            "Invalid ID format. Please enter a valid number."
                        );
                        continue;
                    }
                    if (customerId <= 0)
                    {
                        ConsoleUIHelper.DisplayError("ID must be greater than 0.");
                        continue;
                    }
                    break;
                } while (true);

                // Get project manager ID with validation
                int projectManagerId;
                do
                {
                    var projectManagerIdStr = ConsoleUIHelper.GetInput(
                        "Project Manager ID",
                        ConsoleColor.Cyan
                    );
                    if (!int.TryParse(projectManagerIdStr, out projectManagerId))
                    {
                        ConsoleUIHelper.DisplayError(
                            "Invalid ID format. Please enter a valid number."
                        );
                        continue;
                    }
                    if (projectManagerId <= 0)
                    {
                        ConsoleUIHelper.DisplayError("ID must be greater than 0.");
                        continue;
                    }
                    break;
                } while (true);

                // Get service ID with validation
                int serviceId;
                do
                {
                    var serviceIdStr = ConsoleUIHelper.GetInput("Service ID", ConsoleColor.Cyan);
                    if (!int.TryParse(serviceIdStr, out serviceId))
                    {
                        ConsoleUIHelper.DisplayError(
                            "Invalid ID format. Please enter a valid number."
                        );
                        continue;
                    }
                    if (serviceId <= 0)
                    {
                        ConsoleUIHelper.DisplayError("ID must be greater than 0.");
                        continue;
                    }
                    break;
                } while (true);

                // Get status ID with validation
                int statusId;
                do
                {
                    var statusIdStr = ConsoleUIHelper.GetInput("Status ID", ConsoleColor.Cyan);
                    if (!int.TryParse(statusIdStr, out statusId))
                    {
                        ConsoleUIHelper.DisplayError(
                            "Invalid ID format. Please enter a valid number."
                        );
                        continue;
                    }
                    if (statusId <= 0)
                    {
                        ConsoleUIHelper.DisplayError("ID must be greater than 0.");
                        continue;
                    }
                    break;
                } while (true);

                ConsoleUIHelper.ShowLoadingSpinner(
                    "Creating project",
                    async () =>
                    {
                        var request = new CreateProjectRequest
                        {
                            Name = name,
                            StartDate = startDate,
                            EndDate = endDate,
                            TotalPrice = totalPrice,
                            CustomerId = customerId,
                            ProjectManagerId = projectManagerId,
                            ServiceId = serviceId,
                            StatusId = statusId,
                        };

                        var result = await _projectService.CreateAsync(request);
                        if (result.Success)
                        {
                            ConsoleUIHelper.DisplaySuccess(
                                $"Project created successfully with number {result.Data.ProjectNumber}"
                            );
                        }
                        else
                        {
                            ConsoleUIHelper.DisplayError(
                                $"Failed to create project: {result.Message}"
                            );
                        }
                    }
                );

                ConsoleUIHelper.PressAnyKey();
            }
            catch (Exception ex)
            {
                ConsoleUIHelper.DisplayError($"An unexpected error occurred: {ex.Message}");
                ConsoleUIHelper.PressAnyKey();
            }
        }

        private async Task EditProject()
        {
            ConsoleHelper.DisplayHeader("Edit Project");

            var id = ConsoleHelper.GetId("Enter Project ID");

            var projectResult = await _projectService.GetByIdAsync(id);
            if (!projectResult.Success || projectResult.Data is null)
            {
                ConsoleHelper.DisplayError(projectResult.Message);
                return;
            }

            var project = projectResult.Data;
            Console.WriteLine($"\nEditing Project: {project.ProjectNumber} - {project.Name}");

            var name = ConsoleHelper.GetRequiredInput("New Name");
            var startDate = ConsoleHelper.GetDate("New Start Date");
            var endDate = ConsoleHelper.GetDate("New End Date");
            var totalPrice = ConsoleHelper.GetDecimal("New Total Price");
            var customerId = ConsoleHelper.GetId("New Customer ID");
            var projectManagerId = ConsoleHelper.GetId("New Project Manager ID");
            var serviceId = ConsoleHelper.GetId("New Service ID");
            var statusId = ConsoleHelper.GetId("New Status ID");

            var request = new UpdateProjectRequest
            {
                Id = id,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                TotalPrice = totalPrice,
                CustomerId = customerId,
                ProjectManagerId = projectManagerId,
                ServiceId = serviceId,
                StatusId = statusId,
            };

            var result = await _projectService.UpdateAsync(request);
            if (result.Success)
            {
                ConsoleHelper.DisplaySuccess("Project updated successfully");
            }
            else
            {
                ConsoleHelper.DisplayError(result.Message);
            }
        }

        private async Task ViewProjectDetails()
        {
            Console.Clear();
            Console.WriteLine("=== Project Details ===\n");

            Console.Write("Enter Project ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format");
                Console.ReadKey();
                return;
            }

            var result = await _projectService.GetByIdAsync(id);
            if (!result.Success || result.Data is null)
            {
                Console.WriteLine($"\nError: {result.Message}");
                Console.ReadKey();
                return;
            }

            var project = result.Data;
            Console.WriteLine($"\nProject Number: {project.ProjectNumber}");
            Console.WriteLine($"Name: {project.Name}");
            Console.WriteLine($"Start Date: {project.StartDate:yyyy-MM-dd}");
            Console.WriteLine($"End Date: {project.EndDate:yyyy-MM-dd}");
            Console.WriteLine($"Status: {project.Status?.Name ?? "N/A"}");
            Console.WriteLine($"Customer: {project.Customer?.Name ?? "N/A"}");
            Console.WriteLine($"Project Manager: {project.ProjectManager?.Name ?? "N/A"}");
            Console.WriteLine($"Service: {project.Service?.Name ?? "N/A"}");
            Console.WriteLine($"Total Price: {project.TotalPrice:C}");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
