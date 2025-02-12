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
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Validate request
            if (request == null)
                return ServiceResult<Project>.Error("Project request cannot be null");

            var validation = _projectFactory.ValidateCreate(request);
            if (!validation.IsValid)
                return ServiceResult<Project>.Error(
                    $"Validation failed: {string.Join(", ", validation.Errors)}"
                );

            // Additional validations
            if (request.StartDate > request.EndDate)
                return ServiceResult<Project>.Error("Start date cannot be later than end date");

            if (request.TotalPrice < 0)
                return ServiceResult<Project>.Error("Total price cannot be negative");

            // Create entity
            var entity = _projectFactory.CreateEntity(request);

            // Check for duplicate project number
            if (await _projectRepository.ProjectNumberExistsAsync(entity.ProjectNumber))
                return ServiceResult<Project>.Error(
                    $"Project number {entity.ProjectNumber} already exists"
                );

            // Verify related entities exist
            var customerExists = await _context.Customers.FindAsync(request.CustomerId) != null;
            if (!customerExists)
                return ServiceResult<Project>.Error(
                    $"Customer with ID {request.CustomerId} does not exist"
                );

            var projectManagerExists =
                await _context.ProjectManagers.FindAsync(request.ProjectManagerId) != null;
            if (!projectManagerExists)
                return ServiceResult<Project>.Error(
                    $"Project Manager with ID {request.ProjectManagerId} does not exist"
                );

            var serviceExists = await _context.Services.FindAsync(request.ServiceId) != null;
            if (!serviceExists)
                return ServiceResult<Project>.Error(
                    $"Service with ID {request.ServiceId} does not exist"
                );

            var statusExists = await _context.Statuses.FindAsync(request.StatusId) != null;
            if (!statusExists)
                return ServiceResult<Project>.Error(
                    $"Status with ID {request.StatusId} does not exist"
                );

            // Save project
            var project = await _projectRepository.AddAsync(entity);
            await _projectRepository.SaveChangesAsync();

            await transaction.CommitAsync();
            return ServiceResult<Project>.Ok(project, "Project created successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            var errorMessage =
                ex.InnerException != null
                    ? $"Error creating project: {ex.Message}. Inner error: {ex.InnerException.Message}"
                    : $"Error creating project: {ex.Message}";
            return ServiceResult<Project>.Error(errorMessage);
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
