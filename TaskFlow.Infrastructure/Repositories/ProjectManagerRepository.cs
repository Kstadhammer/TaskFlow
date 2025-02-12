using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Infrastructure.Data.Context;

namespace TaskFlow.Infrastructure.Repositories;

public class ProjectManagerRepository : BaseRepository<ProjectManager>, IProjectManagerRepository
{
    public ProjectManagerRepository(TaskFlowDbContext context)
        : base(context) { }

    public override async Task<IEnumerable<ProjectManager>> GetAllAsync()
    {
        return await _entities.Include(pm => pm.Projects).ToListAsync();
    }

    public override async Task<ProjectManager?> GetByIdAsync(int id)
    {
        return await _entities.Include(pm => pm.Projects).FirstOrDefaultAsync(pm => pm.Id == id);
    }
}
