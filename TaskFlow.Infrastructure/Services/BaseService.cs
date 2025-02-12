using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Core.Entities;
using TaskFlow.Core.Interfaces.Factories;
using TaskFlow.Core.Interfaces.Repositories;
using TaskFlow.Core.Models.Responses;
using TaskFlow.Infrastructure.Data.Context;

namespace TaskFlow.Infrastructure.Services
{
    public abstract class BaseService<TEntity, TCreateRequest, TUpdateRequest>
        where TEntity : Base
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IBaseFactory<TEntity, TCreateRequest, TUpdateRequest> _factory;
        protected readonly TaskFlowDbContext _context;

        protected BaseService(
            IBaseRepository<TEntity> repository,
            IBaseFactory<TEntity, TCreateRequest, TUpdateRequest> factory,
            TaskFlowDbContext context
        )
        {
            _repository = repository;
            _factory = factory;
            _context = context;
        }

        public virtual async Task<ServiceResult<IEnumerable<TEntity>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return ServiceResult<IEnumerable<TEntity>>.Ok(entities);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<TEntity>>.Error(
                    $"Error retrieving entities: {ex.Message}"
                );
            }
        }

        public virtual async Task<ServiceResult<TEntity>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return ServiceResult<TEntity>.Error("Entity not found");

                return ServiceResult<TEntity>.Ok(entity);
            }
            catch (Exception ex)
            {
                return ServiceResult<TEntity>.Error($"Error retrieving entity: {ex.Message}");
            }
        }

        public virtual async Task<ServiceResult<TEntity>> CreateAsync(TCreateRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var validation = _factory.ValidateCreate(request);
                if (!validation.IsValid)
                    return ServiceResult<TEntity>.Error(string.Join(", ", validation.Errors));

                var entity = _factory.CreateEntity(request);
                var createdEntity = await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync();
                return ServiceResult<TEntity>.Ok(createdEntity, "Entity created successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ServiceResult<TEntity>.Error($"Error creating entity: {ex.Message}");
            }
        }

        public virtual async Task<ServiceResult<TEntity>> UpdateAsync(TUpdateRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var validation = _factory.ValidateUpdate(request);
                if (!validation.IsValid)
                    return ServiceResult<TEntity>.Error(string.Join(", ", validation.Errors));

                var entity = await _repository.GetByIdAsync(GetIdFromRequest(request));
                if (entity == null)
                    return ServiceResult<TEntity>.Error("Entity not found");

                _factory.UpdateEntity(entity, request);
                await _repository.UpdateAsync(entity);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync();
                return ServiceResult<TEntity>.Ok(entity, "Entity updated successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ServiceResult<TEntity>.Error($"Error updating entity: {ex.Message}");
            }
        }

        public virtual async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return ServiceResult<bool>.Error("Entity not found");

                var result = await _repository.DeleteAsync(id);
                if (!result)
                    return ServiceResult<bool>.Error("Failed to delete entity");

                await _repository.SaveChangesAsync();

                await transaction.CommitAsync();
                return ServiceResult<bool>.Ok(true, "Entity deleted successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ServiceResult<bool>.Error($"Error deleting entity: {ex.Message}");
            }
        }

        protected abstract int GetIdFromRequest(TUpdateRequest request);
    }
}
