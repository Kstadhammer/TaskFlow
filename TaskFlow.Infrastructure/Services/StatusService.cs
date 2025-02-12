using System;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Core.Interfaces.Services;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Core.Models.Responses;
using TaskFlow.Infrastructure.Data.Context;

namespace TaskFlow.Infrastructure.Services;

public class StatusService
    : BaseService<Status, CreateStatusRequest, UpdateStatusRequest>,
        IStatusService
{
    private readonly IStatusRepository _statusRepository;
    private readonly IStatusFactory _statusFactory;

    public StatusService(
        IStatusRepository statusRepository,
        IStatusFactory statusFactory,
        TaskFlowDbContext context
    )
        : base(statusRepository, statusFactory, context)
    {
        _statusRepository = statusRepository;
        _statusFactory = statusFactory;
    }

    protected override int GetIdFromRequest(UpdateStatusRequest request)
    {
        return request.Id;
    }

    public async Task<ServiceResult<IEnumerable<Status>>> GetAllAsync()
    {
        try
        {
            var statuses = await _statusRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Status>>.Ok(statuses);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<Status>>.Error(
                $"Error retrieving statuses: {ex.Message}"
            );
        }
    }

    public async Task<ServiceResult<Status>> GetByIdAsync(int id)
    {
        try
        {
            var status = await _statusRepository.GetByIdAsync(id);
            if (status == null)
                return ServiceResult<Status>.Error("Status not found");

            return ServiceResult<Status>.Ok(status);
        }
        catch (Exception ex)
        {
            return ServiceResult<Status>.Error($"Error retrieving status: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Status>> CreateAsync(CreateStatusRequest request)
    {
        try
        {
            var validation = _statusFactory.ValidateCreate(request);
            if (!validation.IsValid)
                return ServiceResult<Status>.Error(string.Join(", ", validation.Errors));

            if (await _statusRepository.NameExistsAsync(request.Name))
                return ServiceResult<Status>.Error("A status with this name already exists");

            var entity = _statusFactory.CreateEntity(request);
            var status = await _statusRepository.AddAsync(entity);
            await _statusRepository.SaveChangesAsync();

            return ServiceResult<Status>.Ok(status, "Status created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<Status>.Error($"Error creating status: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Status>> UpdateAsync(UpdateStatusRequest request)
    {
        try
        {
            var validation = _statusFactory.ValidateUpdate(request);
            if (!validation.IsValid)
                return ServiceResult<Status>.Error(string.Join(", ", validation.Errors));

            var status = await _statusRepository.GetByIdAsync(request.Id);
            if (status == null)
                return ServiceResult<Status>.Error("Status not found");

            if (
                status.Name != request.Name
                && await _statusRepository.NameExistsAsync(request.Name)
            )
                return ServiceResult<Status>.Error("A status with this name already exists");

            _statusFactory.UpdateEntity(status, request);
            await _statusRepository.UpdateAsync(status);
            await _statusRepository.SaveChangesAsync();

            return ServiceResult<Status>.Ok(status, "Status updated successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<Status>.Error($"Error updating status: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var status = await _statusRepository.GetByIdAsync(id);
            if (status == null)
                return ServiceResult<bool>.Error("Status not found");

            if (status.Projects.Any())
                return ServiceResult<bool>.Error(
                    "Cannot delete status that is being used by projects"
                );

            var result = await _statusRepository.DeleteAsync(id);
            if (!result)
                return ServiceResult<bool>.Error("Failed to delete status");

            await _statusRepository.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true, "Status deleted successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Error($"Error deleting status: {ex.Message}");
        }
    }
}
