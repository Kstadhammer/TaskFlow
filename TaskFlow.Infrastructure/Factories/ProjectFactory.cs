using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Models.Requests;
using TaskFlow.Core.Models.Validation;

namespace TaskFlow.Infrastructure.Factories;

public class ProjectFactory : IProjectFactory
{
    public ValidationResult ValidateCreate(CreateProjectRequest request)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Project name is required");

        if (request.StartDate > request.EndDate)
            result.AddError("Start date must be before end date");

        if (request.TotalPrice < 0)
            result.AddError("Total price cannot be negative");

        if (request.CustomerId <= 0)
            result.AddError("Valid customer is required");

        if (request.ProjectManagerId <= 0)
            result.AddError("Valid project manager is required");

        if (request.ServiceId <= 0)
            result.AddError("Valid service is required");

        if (request.StatusId <= 0)
            result.AddError("Valid status is required");

        return result;
    }

    public ValidationResult ValidateUpdate(UpdateProjectRequest request)
    {
        var result = new ValidationResult();

        if (request.Id <= 0)
            result.AddError("Valid project ID is required");

        if (string.IsNullOrWhiteSpace(request.Name))
            result.AddError("Project name is required");

        if (request.StartDate > request.EndDate)
            result.AddError("Start date must be before end date");

        if (request.TotalPrice < 0)
            result.AddError("Total price cannot be negative");

        if (request.CustomerId <= 0)
            result.AddError("Valid customer is required");

        if (request.ProjectManagerId <= 0)
            result.AddError("Valid project manager is required");

        if (request.ServiceId <= 0)
            result.AddError("Valid service is required");

        if (request.StatusId <= 0)
            result.AddError("Valid status is required");

        return result;
    }

    public Project CreateEntity(CreateProjectRequest request)
    {
        return new Project
        {
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalPrice = request.TotalPrice,
            StatusId = request.StatusId,
            CustomerId = request.CustomerId,
            ProjectManagerId = request.ProjectManagerId,
            ServiceId = request.ServiceId,
            ProjectNumber = GenerateProjectNumber(),
        };
    }

    public void UpdateEntity(Project entity, UpdateProjectRequest request)
    {
        entity.Name = request.Name;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.TotalPrice = request.TotalPrice;
        entity.StatusId = request.StatusId;
        entity.CustomerId = request.CustomerId;
        entity.ProjectManagerId = request.ProjectManagerId;
        entity.ServiceId = request.ServiceId;
    }

    private string GenerateProjectNumber()
    {
        // Generate a unique project number in format "P-YYYYMMDD-XXX"
        // This format includes the date and a sequence number
        var date = DateTime.Now.ToString("yyyyMMdd");
        var random = new Random();
        var sequence = random.Next(1, 1000).ToString("000");
        return $"P-{date}-{sequence}";
    }
}
