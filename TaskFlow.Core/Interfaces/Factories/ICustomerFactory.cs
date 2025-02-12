using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Requests;

namespace TaskFlow.Core.Interfaces.Factories;

public interface ICustomerFactory
    : IBaseFactory<Customer, CreateCustomerRequest, UpdateCustomerRequest>
{
    // Add any customer-specific factory methods here if needed
}
