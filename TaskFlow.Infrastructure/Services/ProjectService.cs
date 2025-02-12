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

public class ProjectService
    : BaseService<Project, CreateProjectRequest, UpdateProjectRequest>,
        IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectFactory _projectFactory;

    public ProjectService(
        IProjectRepository projectRepository,
        IProjectFactory projectFactory,
        TaskFlowDbContext context
    )
        : base(projectRepository, projectFactory, context)
    {
        _projectRepository = projectRepository;
        _projectFactory = projectFactory;
    }

    protected override int GetIdFromRequest(UpdateProjectRequest request)
    {
        return request.Id;
    }

    public override async Task<ServiceResult<Project>> CreateAsync(CreateProjectRequest request)
    {
        try
        {
            var validation = _projectFactory.ValidateCreate(request);
            if (!validation.IsValid)
                return ServiceResult<Project>.Error(string.Join(", ", validation.Errors));

            var entity = _projectFactory.CreateEntity(request);

            if (await _projectRepository.ProjectNumberExistsAsync(entity.ProjectNumber))
                return ServiceResult<Project>.Error("Project number already exists");

            var project = await _projectRepository.AddAsync(entity);
            await _projectRepository.SaveChangesAsync();

            return ServiceResult<Project>.Ok(project, "Project created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<Project>.Error($"Error creating project: {ex.Message}");
        }
    }

    public async Task<ServiceResult<IEnumerable<Project>>> GetAllAsync()
    {
        try
        {
            var projects = await _projectRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Project>>.Ok(projects);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<Project>>.Error(
                $"Error retrieving projects: {ex.Message}"
            );
        }
    }

    public async Task<ServiceResult<Project>> GetByIdAsync(int id)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return ServiceResult<Project>.Error("Project not found");

            return ServiceResult<Project>.Ok(project);
        }
        catch (Exception ex)
        {
            return ServiceResult<Project>.Error($"Error retrieving project: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Project>> UpdateAsync(UpdateProjectRequest request)
    {
        try
        {
            var validation = _projectFactory.ValidateUpdate(request);
            if (!validation.IsValid)
                return ServiceResult<Project>.Error(string.Join(", ", validation.Errors));

            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
                return ServiceResult<Project>.Error("Project not found");

            _projectFactory.UpdateEntity(project, request);
            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();

            return ServiceResult<Project>.Ok(project, "Project updated successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<Project>.Error($"Error updating project: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return ServiceResult<bool>.Error("Project not found");

            var result = await _projectRepository.DeleteAsync(id);
            if (!result)
                return ServiceResult<bool>.Error("Failed to delete project");

            await _projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true, "Project deleted successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Error($"Error deleting project: {ex.Message}");
        }
    }
}
