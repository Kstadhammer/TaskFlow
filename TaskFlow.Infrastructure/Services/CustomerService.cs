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

public class CustomerService
    : BaseService<Customer, CreateCustomerRequest, UpdateCustomerRequest>,
        ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(
        ICustomerRepository repository,
        ICustomerFactory factory,
        TaskFlowDbContext context
    )
        : base(repository, factory, context)
    {
        _customerRepository = repository;
    }

    protected override int GetIdFromRequest(UpdateCustomerRequest request)
    {
        return request.Id;
    }

    public async Task<ServiceResult<IEnumerable<Customer>>> GetAllAsync()
    {
        try
        {
            var customers = await _customerRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Customer>>.Ok(customers);
        }
        catch (Exception ex)
        {
            return ServiceResult<IEnumerable<Customer>>.Error(
                $"Error retrieving customers: {ex.Message}"
            );
        }
    }

    public async Task<ServiceResult<Customer>> GetByIdAsync(int id)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return ServiceResult<Customer>.Error("Customer not found");

            return ServiceResult<Customer>.Ok(customer);
        }
        catch (Exception ex)
        {
            return ServiceResult<Customer>.Error($"Error retrieving customer: {ex.Message}");
        }
    }

    public override async Task<ServiceResult<Customer>> CreateAsync(CreateCustomerRequest request)
    {
        if (await _customerRepository.EmailExistsAsync(request.Email))
            return ServiceResult<Customer>.Error("A customer with this email already exists");

        return await base.CreateAsync(request);
    }

    public override async Task<ServiceResult<Customer>> UpdateAsync(UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (
            customer != null
            && customer.Email != request.Email
            && await _customerRepository.EmailExistsAsync(request.Email)
        )
            return ServiceResult<Customer>.Error("A customer with this email already exists");

        return await base.UpdateAsync(request);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return ServiceResult<bool>.Error("Customer not found");

            if (customer.Projects.Any())
                return ServiceResult<bool>.Error(
                    "Cannot delete customer that has associated projects"
                );

            var result = await _customerRepository.DeleteAsync(id);
            if (!result)
                return ServiceResult<bool>.Error("Failed to delete customer");

            await _customerRepository.SaveChangesAsync();
            return ServiceResult<bool>.Ok(true, "Customer deleted successfully");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Error($"Error deleting customer: {ex.Message}");
        }
    }
}
