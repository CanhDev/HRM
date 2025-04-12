using ERP.Base_sys.Repos;
using ERP.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP.Base_sys.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IBaseRepository<T> _repository;
        protected readonly ApplicationDbContext _context;

        public BaseService(IBaseRepository<T> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<bool> IsDuplicateAsync(Expression<Func<T, bool>> filter)
        {
            return await _repository.AnyAsync(filter);
        }

        public virtual async Task<ApiResponse<T?>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? ApiResponse<T?>.SuccessResponse(entity) : ApiResponse<T?>.ErrorResponse("Not found");
        }

        public virtual async Task<ApiResponse<List<T>>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return ApiResponse<List<T>>.SuccessResponse(entities);
        }

        public virtual async Task<ApiResponse<PagedResult<T>>> GetPagedListAsync(
            SearchRequest<T> searchRequest,params Expression<Func<T, object>>[] includes)
        {
            var result = await _repository.GetPagedListAsync(searchRequest, includes);
            return ApiResponse<PagedResult<T>>.SuccessResponse(result);
        }

        public virtual async Task<ApiResponse<T>> AddAsync(T entity, Expression<Func<T, bool>> duplicateCheckFilter)
        {
            if (await IsDuplicateAsync(duplicateCheckFilter))
            {
                throw new InvalidOperationException("Entity already exists.");
            }
            var result = await _repository.AddAsync(entity);
            return ApiResponse<T>.SuccessResponse(result);
        }

        public virtual async Task<ApiResponse<T>> UpdateAsync(T entity, Expression<Func<T, bool>> duplicateCheckFilter)
        {
            if (await IsDuplicateAsync(duplicateCheckFilter) == false)
            {
                throw new InvalidOperationException("Entity does not exists.");
            }
            var result = await _repository.UpdateAsync(entity);
            return ApiResponse<T>.SuccessResponse(result);
        }

        public virtual async Task<ApiResponse<T>> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return ApiResponse<T>.ErrorResponse("Not found");
            var res =  await _repository.DeleteAsync(entity);
            return ApiResponse<T>.SuccessResponse(res);
        }

        public virtual async Task<ApiResponse<string>> AddTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2)
            where T1 : class
            where T2 : class
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Set<T1>().AddAsync(entity1);
                await _context.Set<T2>().AddAsync(entity2);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ApiResponse<string>.SuccessResponse("Both entities added successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<string>.ErrorResponse($"Transaction failed: {ex.Message}");
            }
        }

        public virtual async Task<ApiResponse<string>> UpdateTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2)
            where T1 : class
            where T2 : class
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Set<T1>().Update(entity1);
                _context.Set<T2>().Update(entity2);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ApiResponse<string>.SuccessResponse("Both entities updated successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<string>.ErrorResponse($"Update transaction failed: {ex.Message}");
            }
        }

        public virtual async Task<ApiResponse<string>> DeleteTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2)
            where T1 : class
            where T2 : class
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Set<T1>().Remove(entity1);
                _context.Set<T2>().Remove(entity2);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ApiResponse<string>.SuccessResponse("Both entities deleted successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<string>.ErrorResponse($"Delete transaction failed: {ex.Message}");
            }
        }

        public virtual async Task<ApiResponse<string>> DeleteTwoEntitiesByIdAsync<T1, T2>(int id1, int id2)
        where T1 : class
        where T2 : class
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity1 = await _context.Set<T1>().FindAsync(id1);
                var entity2 = await _context.Set<T2>().FindAsync(id2);

                if (entity1 == null || entity2 == null)
                {
                    await transaction.RollbackAsync();
                    return ApiResponse<string>.ErrorResponse("One or both entities not found");
                }

                _context.Set<T1>().Remove(entity1);
                _context.Set<T2>().Remove(entity2);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ApiResponse<string>.SuccessResponse("Both entities deleted successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<string>.ErrorResponse($"Delete by ID transaction failed: {ex.Message}");
            }
        }

        public virtual async Task<ApiResponse<string>> UpdateRelatedEntitiesAsync<TParent, TChild>(
            TParent parent,
            List<TChild> childrenToAdd,
            List<TChild> childrenToUpdate,
            List<int> childIdsToDelete)
            where TParent : class
            where TChild : class
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update parent entity
                _context.Set<TParent>().Update(parent);

                // Add new children
                foreach (var child in childrenToAdd)
                {
                    await _context.Set<TChild>().AddAsync(child);
                }

                // Update existing children
                foreach (var child in childrenToUpdate)
                {
                    _context.Set<TChild>().Update(child);
                }

                // Delete children by ids
                foreach (var id in childIdsToDelete)
                {
                    var entityToDelete = await _context.Set<TChild>().FindAsync(id);
                    if (entityToDelete != null)
                    {
                        _context.Set<TChild>().Remove(entityToDelete);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return ApiResponse<string>.SuccessResponse("Parent and related children updated successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<string>.ErrorResponse($"Related entities update transaction failed: {ex.Message}");
            }
        }
    }
}
