using System.Text.RegularExpressions;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Core.Models.Validation;

namespace TaskFlow.Infrastructure.Factories;

public class CustomerFactory : ICustomerFactory
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled
    );
    private static readonly Regex PhoneRegex = new(@"^\+?[\d\s-]{8,}$", RegexOptions.Compiled);

    public ValidationResult ValidateCreate(CreateCustomerRequest request)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Customer name is required");

        if (string.IsNullOrWhiteSpace(request.Email))
            result.AddError("Email is required");
        else if (!EmailRegex.IsMatch(request.Email))
            result.AddError("Invalid email format");

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            result.AddError("Phone number is required");
        else if (!PhoneRegex.IsMatch(request.PhoneNumber))
            result.AddError("Invalid phone number format");

        return result;
    }

    public ValidationResult ValidateUpdate(UpdateCustomerRequest request)
    {
        var result = new ValidationResult();

        if (request.Id <= 0)
            result.AddError("Valid customer ID is required");

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Customer name is required");

        if (string.IsNullOrWhiteSpace(request.Email))
            result.AddError("Email is required");
        else if (!EmailRegex.IsMatch(request.Email))
            result.AddError("Invalid email format");

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            result.AddError("Phone number is required");
        else if (!PhoneRegex.IsMatch(request.PhoneNumber))
            result.AddError("Invalid phone number format");

        return result;
    }

    public Customer CreateEntity(CreateCustomerRequest request)
    {
        return new Customer
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
        };
    }

    public void UpdateEntity(Customer entity, UpdateCustomerRequest request)
    {
        entity.Name = request.Name;
        entity.Email = request.Email;
        entity.PhoneNumber = request.PhoneNumber;
    }
}
