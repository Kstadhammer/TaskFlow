using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Services;

public interface IProjectManagerService
    : IBaseService<ProjectManager, CreateProjectManagerRequest, UpdateProjectManagerRequest>
{
    // Add any project manager-specific service methods here if needed
}
