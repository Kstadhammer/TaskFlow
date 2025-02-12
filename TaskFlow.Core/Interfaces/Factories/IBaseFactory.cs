using TaskFlow.Core.Entities;
using TaskFlow.Core.Models.Validation;

namespace TaskFlow.Core.Interfaces.Factories;

public interface IBaseFactory<TEntity, TCreateRequest, TUpdateRequest>
    where TEntity : Base
{
    ValidationResult ValidateCreate(TCreateRequest request);
    ValidationResult ValidateUpdate(TUpdateRequest request);
    TEntity CreateEntity(TCreateRequest request);
    void UpdateEntity(TEntity entity, TUpdateRequest request);
}
