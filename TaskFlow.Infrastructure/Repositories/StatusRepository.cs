using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Infrastructure.Data.Context;

namespace TaskFlow.Infrastructure.Repositories;

public class StatusRepository : BaseRepository<Status>, IStatusRepository
{
    public StatusRepository(TaskFlowDbContext context)
        : base(context) { }

    public override async Task<IEnumerable<Status>> GetAllAsync()
    {
        return await _entities.Include(s => s.Projects).ToListAsync();
    }

    public override async Task<Status?> GetByIdAsync(int id)
    {
        return await _entities.Include(s => s.Projects).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Status?> GetByNameAsync(string name)
    {
        return await _entities.Include(s => s.Projects).FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _entities.AnyAsync(s => s.Name == name);
    }

    public async Task<IEnumerable<Status>> GetStatusesWithProjectCountAsync()
    {
        return await _entities
            .Include(s => s.Projects)
            .Select(s => new Status
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Projects = s.Projects,
            })
            .ToListAsync();
    }
}
