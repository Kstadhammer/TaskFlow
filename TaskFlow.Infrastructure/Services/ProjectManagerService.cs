using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Core.Models.Responses;
using TaskFlow.Infrastructure.Data.Context;

namespace TaskFlow.Infrastructure.Services;

public class ProjectManagerService
    : BaseService<ProjectManager, CreateProjectManagerRequest, UpdateProjectManagerRequest>,
        IProjectManagerService
{
    private readonly IProjectManagerRepository _projectManagerRepository;
    private readonly IProjectManagerFactory _projectManagerFactory;

    public ProjectManagerService(
        IProjectManagerRepository projectManagerRepository,
        IProjectManagerFactory projectManagerFactory,
        TaskFlowDbContext context
    )
        : base(projectManagerRepository, projectManagerFactory, context)
    {
        _projectManagerRepository = projectManagerRepository;
        _projectManagerFactory = projectManagerFactory;
    }

    protected override int GetIdFromRequest(UpdateProjectManagerRequest request)
    {
        return request.Id;
    }

    public async Task<ServiceResult<IEnumerable<ProjectManager>>> GetAllAsync()
    {
        try
        {
            var projectManagers = await _projectManagerRepository.GetAllAsync();
            return ServiceResult<IEnumerable<ProjectManager>>.Ok(projectManagers);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<ProjectManager>>.Error(
                $"Error retrieving project managers: {ex.Message}"
            );
        }
    }

    public async Task<ServiceResult<ProjectManager>> GetByIdAsync(int id)
    {
        try
        {
            var projectManager = await _projectManagerRepository.GetByIdAsync(id);
            if (projectManager == null)
                return ServiceResult<ProjectManager>.Error("Project manager not found");

            return ServiceResult<ProjectManager>.Ok(projectManager);
        }
        catch (Exception ex)
        {
            return ServiceResult<ProjectManager>.Error(
                $"Error retrieving project manager: {ex.Message}"
            );
        }
    }

    public async Task<ServiceResult<ProjectManager>> CreateAsync(
        CreateProjectManagerRequest request
    )
    {
        try
        {
            var validation = _projectManagerFactory.ValidateCreate(request);
            if (!validation.IsValid)
                return ServiceResult<ProjectManager>.Error(string.Join(", ", validation.Errors));

            var entity = _projectManagerFactory.CreateEntity(request);
            var projectManager = await _projectManagerRepository.AddAsync(entity);
            await _projectManagerRepository.SaveChangesAsync();

            return ServiceResult<ProjectManager>.Ok(
                projectManager,
                "Project manager created successfully"
            );
        }
        catch (Exception ex)
        {
            return ServiceResult<ProjectManager>.Error(
                $"Error creating project manager: {ex.Message}"
            );
        }
    }

    public async Task<ServiceResult<ProjectManager>> UpdateAsync(
        UpdateProjectManagerRequest request
    )
    {
        try
        {
            var validation = _projectManagerFactory.ValidateUpdate(request);
            if (!validation.IsValid)
                return ServiceResult<ProjectManager>.Error(string.Join(", ", validation.Errors));

            var projectManager = await _projectManagerRepository.GetByIdAsync(request.Id);
            if (projectManager == null)
                return ServiceResult<ProjectManager>.Error("Project manager not found");

            _projectManagerFactory.UpdateEntity(projectManager, request);
            await _projectManagerRepository.UpdateAsync(projectManager);
            await _projectManagerRepository.SaveChangesAsync();

            return ServiceResult<ProjectManager>.Ok(
                projectManager,
                "Project manager updated successfully"
            );
        }
        catch (Exception ex)
        {
            return ServiceResult<ProjectManager>.Error(
                $"Error updating project manager: {ex.Message}"
            );
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var projectManager = await _projectManagerRepository.GetByIdAsync(id);
            if (projectManager == null)
                return ServiceResult<bool>.Error("Project manager not found");

            if (projectManager.Projects.Any())
                return ServiceResult<bool>.Error(
                    "Cannot delete project manager that has associated projects"
                );

            var result = await _projectManagerRepository.DeleteAsync(id);
            if (!result)
                return ServiceResult<bool>.Error("Failed to delete project manager");

            await _projectManagerRepository.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true, "Project manager deleted successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Error($"Error deleting project manager: {ex.Message}");
        }
    }
}
