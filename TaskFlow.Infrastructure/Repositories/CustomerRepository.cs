using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Infrastructure.Data.Context;

namespace TaskFlow.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(TaskFlowDbContext context)
        : base(context) { }

    public override async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _entities.Include(c => c.Projects).ToListAsync();
    }

    public override async Task<Customer?> GetByIdAsync(int id)
    {
        return await _entities.Include(c => c.Projects).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _entities.Include(c => c.Projects).FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _entities.AnyAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithActiveProjectsAsync()
    {
        return await _entities
            .Include(c => c.Projects)
            .ThenInclude(p => p.Status)
            .Where(c => c.Projects.Any(p => p.Status.Name != "Completed"))
            .ToListAsync();
    }
}
