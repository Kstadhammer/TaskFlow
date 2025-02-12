using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Presentation.UI.Helpers;

namespace TaskFlow.Presentation.UI;

public class MainMenu
{
    private readonly IProjectService _projectService;
    private readonly ICustomerService _customerService;
    private readonly IProjectManagerService _projectManagerService;
    private readonly IServiceEntityService _serviceEntityService;
    private readonly IStatusService _statusService;

    public MainMenu(
        IProjectService projectService,
        ICustomerService customerService,
        IProjectManagerService projectManagerService,
        IServiceEntityService serviceEntityService,
        IStatusService statusService
    )
    {
        _projectService = projectService;
        _customerService = customerService;
        _projectManagerService = projectManagerService;
        _serviceEntityService = serviceEntityService;
        _statusService = statusService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            var options = new Dictionary<string, string>
            {
                { "1", "Projects" },
                { "2", "Customers" },
                { "3", "Project Managers" },
                { "4", "Services" },
                { "5", "Status Management" },
                { "0", "Exit" },
            };

            var choice = MenuHelper.ShowMenu("TaskFlow Main Menu", options);

            switch (choice)
            {
                case "1":
                    await new ProjectMenu(_projectService).ShowAsync();
                    break;
                case "2":
                    await new CustomerMenu(_customerService).ShowAsync();
                    break;
                case "3":
                    await new ProjectManagerMenu(_projectManagerService).ShowAsync();
                    break;
                case "4":
                    await new ServiceMenu(_serviceEntityService).ShowAsync();
                    break;
                case "5":
                    await new StatusMenu(_statusService).ShowAsync();
                    break;
                case "0":
                    return;
            }
        }
    }
}
