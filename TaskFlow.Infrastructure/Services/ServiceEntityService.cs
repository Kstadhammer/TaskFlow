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

public class ServiceEntityService
    : BaseService<Service, CreateServiceRequest, UpdateServiceRequest>,
        IServiceEntityService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceFactory _serviceFactory;

    public ServiceEntityService(
        IServiceRepository serviceRepository,
        IServiceFactory serviceFactory,
        TaskFlowDbContext context
    )
        : base(serviceRepository, serviceFactory, context)
    {
        _serviceRepository = serviceRepository;
        _serviceFactory = serviceFactory;
    }

    protected override int GetIdFromRequest(UpdateServiceRequest request)
    {
        return request.Id;
    }

    public async Task<ServiceResult<IEnumerable<Service>>> GetAllAsync()
    {
        try
        {
            var services = await _serviceRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Service>>.Ok(services);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<Service>>.Error(
                $"Error retrieving services: {ex.Message}"
            );
        }
    }

    public async Task<ServiceResult<Service>> GetByIdAsync(int id)
    {
        try
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
                return ServiceResult<Service>.Error("Service not found");

            return ServiceResult<Service>.Ok(service);
        }
        catch (Exception ex)
        {
            return ServiceResult<Service>.Error($"Error retrieving service: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Service>> CreateAsync(CreateServiceRequest request)
    {
        try
        {
            var validation = _serviceFactory.ValidateCreate(request);
            if (!validation.IsValid)
                return ServiceResult<Service>.Error(string.Join(", ", validation.Errors));

            var entity = _serviceFactory.CreateEntity(request);
            var service = await _serviceRepository.AddAsync(entity);
            await _serviceRepository.SaveChangesAsync();

            return ServiceResult<Service>.Ok(service, "Service created successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<Service>.Error($"Error creating service: {ex.Message}");
        }
    }

    public async Task<ServiceResult<Service>> UpdateAsync(UpdateServiceRequest request)
    {
        try
        {
            var validation = _serviceFactory.ValidateUpdate(request);
            if (!validation.IsValid)
                return ServiceResult<Service>.Error(string.Join(", ", validation.Errors));

            var service = await _serviceRepository.GetByIdAsync(request.Id);
            if (service == null)
                return ServiceResult<Service>.Error("Service not found");

            _serviceFactory.UpdateEntity(service, request);
            await _serviceRepository.UpdateAsync(service);
            await _serviceRepository.SaveChangesAsync();

            return ServiceResult<Service>.Ok(service, "Service updated successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<Service>.Error($"Error updating service: {ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
                return ServiceResult<bool>.Error("Service not found");

            if (service.Projects.Any())
                return ServiceResult<bool>.Error(
                    "Cannot delete service that is being used by projects"
                );

            var result = await _serviceRepository.DeleteAsync(id);
            if (!result)
                return ServiceResult<bool>.Error("Failed to delete service");

            await _serviceRepository.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true, "Service deleted successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Error($"Error deleting service: {ex.Message}");
        }
    }
}
