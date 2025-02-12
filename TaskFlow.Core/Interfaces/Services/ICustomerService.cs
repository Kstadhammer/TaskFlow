using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Services;

public interface ICustomerService
    : IBaseService<Customer, CreateCustomerRequest, UpdateCustomerRequest>
{
    // Add any customer-specific methods here if needed
}
