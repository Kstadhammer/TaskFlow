using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Core.Models.Validation;

namespace TaskFlow.Infrastructure.Factories;

public class StatusFactory : IStatusFactory
{
    public ValidationResult ValidateCreate(CreateStatusRequest request)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Status name is required");

        if (string.IsNullOrWhiteSpace(request.Description))
            result.AddError("Status description is required");

        return result;
    }

    public ValidationResult ValidateUpdate(UpdateStatusRequest request)
    {
        var result = new ValidationResult();

        if (request.Id <= 0)
            result.AddError("Valid status ID is required");

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Status name is required");

        if (string.IsNullOrWhiteSpace(request.Description))
            result.AddError("Status description is required");

        return result;
    }

    public Status CreateEntity(CreateStatusRequest request)
    {
        return new Status { Name = request.Name, Description = request.Description };
    }

    public void UpdateEntity(Status entity, UpdateStatusRequest request)
    {
        entity.Name = request.Name;
        entity.Description = request.Description;
    }
}
