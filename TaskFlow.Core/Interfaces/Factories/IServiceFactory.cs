using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Factories;

public interface IServiceFactory : IBaseFactory<Service, CreateServiceRequest, UpdateServiceRequest>
{
    // Add any service-specific factory methods here if needed
}
