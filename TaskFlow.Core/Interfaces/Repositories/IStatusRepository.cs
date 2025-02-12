using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces.Repositories;

public interface IStatusRepository : IBaseRepository<Status>
{
    Task<Status?> GetByNameAsync(string name);
    Task<bool> NameExistsAsync(string name);
}
