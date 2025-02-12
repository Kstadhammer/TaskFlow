using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Services;

public interface IProjectService : IBaseService<Project, CreateProjectRequest, UpdateProjectRequest>
{
    // Add any project-specific service methods here if needed
}
