using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Core.Models.Validation;

namespace TaskFlow.Infrastructure.Factories;

public class ServiceFactory : IServiceFactory
{
    public ValidationResult ValidateCreate(CreateServiceRequest request)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Service name is required");

        if (string.IsNullOrWhiteSpace(request.Description))
            result.AddError("Service description is required");

        if (request.HourlyRate <= 0)
            result.AddError("Hourly rate must be greater than zero");

        return result;
    }

    public ValidationResult ValidateUpdate(UpdateServiceRequest request)
    {
        var result = new ValidationResult();

        if (request.Id <= 0)
            result.AddError("Valid service ID is required");

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Service name is required");

        if (string.IsNullOrWhiteSpace(request.Description))
            result.AddError("Service description is required");

        if (request.HourlyRate <= 0)
            result.AddError("Hourly rate must be greater than zero");

        return result;
    }

    public Service CreateEntity(CreateServiceRequest request)
    {
        return new Service
        {
            Name = request.Name,
            Description = request.Description,
            HourlyRate = request.HourlyRate,
            IsActive = request.IsActive,
        };
    }

    public void UpdateEntity(Service entity, UpdateServiceRequest request)
    {
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.HourlyRate = request.HourlyRate;
        entity.IsActive = request.IsActive;
    }
}
