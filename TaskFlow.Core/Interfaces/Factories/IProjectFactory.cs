using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Factories;

public interface IProjectFactory : IBaseFactory<Project, CreateProjectRequest, UpdateProjectRequest>
{
    // Add any project-specific factory methods here if needed
}
