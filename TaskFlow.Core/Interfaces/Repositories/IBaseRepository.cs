using TaskFlow.Core.Entities;

namespace TaskFlow.Core.Interfaces.Repositories;

public interface IBaseRepository<TEntity>
    where TEntity : Base
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> SaveChangesAsync();
}
