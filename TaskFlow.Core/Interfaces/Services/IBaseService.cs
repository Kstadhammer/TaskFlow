using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Responses;

namespace TaskFlow.Core.Interfaces.Services;

public interface IBaseService<TEntity, TCreateRequest, TUpdateRequest>
    where TEntity : Base
{
    Task<ServiceResult<IEnumerable<TEntity>>> GetAllAsync();
    Task<ServiceResult<TEntity>> GetByIdAsync(int id);
    Task<ServiceResult<TEntity>> CreateAsync(TCreateRequest request);
    Task<ServiceResult<TEntity>> UpdateAsync(TUpdateRequest request);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
