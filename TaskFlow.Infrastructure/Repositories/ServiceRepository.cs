using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Infrastructure.Data.Context;

namespace TaskFlow.Infrastructure.Repositories;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    public ServiceRepository(TaskFlowDbContext context)
        : base(context) { }

    public override async Task<IEnumerable<Service>> GetAllAsync()
    {
        return await _entities.Include(s => s.Projects).ToListAsync();
    }

    public override async Task<Service?> GetByIdAsync(int id)
    {
        return await _entities.Include(s => s.Projects).FirstOrDefaultAsync(s => s.Id == id);
    }
}
