using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Services;

public interface IServiceEntityService
    : IBaseService<Service, CreateServiceRequest, UpdateServiceRequest>
{
    // Add any service-specific methods here if needed
}
