using System.Text.RegularExpressions;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Core.Models.Validation;

namespace TaskFlow.Infrastructure.Factories;

public class ProjectManagerFactory : IProjectManagerFactory
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled
    );
    private static readonly Regex PhoneRegex = new(@"^\+?[\d\s-]{8,}$", RegexOptions.Compiled);

    public ValidationResult ValidateCreate(CreateProjectManagerRequest request)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Project manager name is required");

        if (string.IsNullOrWhiteSpace(request.Email))
            result.AddError("Email is required");
        else if (!EmailRegex.IsMatch(request.Email))
            result.AddError("Invalid email format");

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            result.AddError("Phone number is required");
        else if (!PhoneRegex.IsMatch(request.PhoneNumber))
            result.AddError("Invalid phone number format");

        if (string.IsNullOrWhiteSpace(request.Department))
            result.AddError("Department is required");

        if (string.IsNullOrWhiteSpace(request.Title))
            result.AddError("Title is required");

        return result;
    }

    public ValidationResult ValidateUpdate(UpdateProjectManagerRequest request)
    {
        var result = new ValidationResult();

        if (request.Id <= 0)
            result.AddError("Valid project manager ID is required");

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Project manager name is required");

        if (string.IsNullOrWhiteSpace(request.Email))
            result.AddError("Email is required");
        else if (!EmailRegex.IsMatch(request.Email))
            result.AddError("Invalid email format");

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            result.AddError("Phone number is required");
        else if (!PhoneRegex.IsMatch(request.PhoneNumber))
            result.AddError("Invalid phone number format");

        if (string.IsNullOrWhiteSpace(request.Department))
            result.AddError("Department is required");

        if (string.IsNullOrWhiteSpace(request.Title))
            result.AddError("Title is required");

        return result;
    }

    public ProjectManager CreateEntity(CreateProjectManagerRequest request)
    {
        return new ProjectManager
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Department = request.Department,
            Title = request.Title,
        };
    }

    public void UpdateEntity(ProjectManager entity, UpdateProjectManagerRequest request)
    {
        entity.Name = request.Name;
        entity.Email = request.Email;
        entity.PhoneNumber = request.PhoneNumber;
        entity.Department = request.Department;
        entity.Title = request.Title;
    }
}
