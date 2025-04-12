using System.Linq.Expressions;

namespace ERP.Base_sys.Services
{
    public interface IBaseService<T> where T : class
    {
        Task<ApiResponse<T?>> GetByIdAsync(int id);
        Task<ApiResponse<List<T>>> GetAllAsync();
        Task<ApiResponse<PagedResult<T>>> GetPagedListAsync(SearchRequest<T> searchRequest,
                                                        params Expression<Func<T, object>>[] includes);
        Task<ApiResponse<T>> AddAsync(T entity, Expression<Func<T, bool>> duplicateCheckFilter);
        Task<ApiResponse<T>> UpdateAsync(T entity, Expression<Func<T, bool>> duplicateCheckFilter);
        Task<ApiResponse<T>> DeleteAsync(int id);
        Task<ApiResponse<string>> AddTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        Task<ApiResponse<string>> UpdateTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        Task<ApiResponse<string>> DeleteTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        Task<ApiResponse<string>> DeleteTwoEntitiesByIdAsync<T1, T2>(int id1, int id2) where T1 : class where T2 : class;
        Task<ApiResponse<string>> UpdateRelatedEntitiesAsync<TParent, TChild>(
            TParent parent,
            List<TChild> childrenToAdd,
            List<TChild> childrenToUpdate,
            List<int> childIdsToDelete)
            where TParent : class
            where TChild : class;
        Task<bool> IsDuplicateAsync(Expression<Func<T, bool>> filter);
    }
}
