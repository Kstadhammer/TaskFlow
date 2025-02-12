using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Infrastructure.Data.Context;

namespace TaskFlow.Infrastructure.Repositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(TaskFlowDbContext context)
        : base(context) { }

    public override async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _entities
            .Include(p => p.Customer)
            .Include(p => p.ProjectManager)
            .Include(p => p.Status)
            .Include(p => p.Service)
            .ToListAsync();
    }

    public override async Task<Project?> GetByIdAsync(int id)
    {
        return await _entities
            .Include(p => p.Customer)
            .Include(p => p.ProjectManager)
            .Include(p => p.Status)
            .Include(p => p.Service)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetByCustomerIdAsync(int customerId)
    {
        return await _entities
            .Include(p => p.Status)
            .Include(p => p.ProjectManager)
            .Where(p => p.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByProjectManagerIdAsync(int projectManagerId)
    {
        return await _entities
            .Include(p => p.Status)
            .Include(p => p.Customer)
            .Where(p => p.ProjectManagerId == projectManagerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByStatusIdAsync(int statusId)
    {
        return await _entities
            .Include(p => p.Customer)
            .Include(p => p.ProjectManager)
            .Where(p => p.StatusId == statusId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        return await _entities
            .Include(p => p.Status)
            .Include(p => p.Customer)
            .Include(p => p.ProjectManager)
            .Where(p => p.StartDate >= startDate && p.EndDate <= endDate)
            .ToListAsync();
    }

    public async Task<bool> ProjectNumberExistsAsync(string projectNumber)
    {
        return await _entities.AnyAsync(p => p.ProjectNumber == projectNumber);
    }
}
