using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Factories;

public interface IProjectManagerFactory
    : IBaseFactory<ProjectManager, CreateProjectManagerRequest, UpdateProjectManagerRequest>
{
    // Add any project manager-specific factory methods here if needed
}
