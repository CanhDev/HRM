using ERP.DTO;
using System.Linq.Expressions;

namespace ERP.Base_sys.Services
{
    public interface IBaseService<T> where T : class
    {
        Task<ApiResponse<T?>> GetByIdAsync(int id);
        Task<ApiResponse<List<T>>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
        Task<ApiResponse<PagedResult<T>>> GetPagedListAsync(SearchRequest<T> searchRequest,
                                                        params Expression<Func<T, object>>[] includes);
        Task<ApiResponse<T>> AddAsync(T entity);
        Task<ApiResponse<T>> UpdateAsync(T entity);
        Task<ApiResponse<T>> DeleteAsync(int id);
        Task<ApiResponse<string>> AddTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        Task<ApiResponse<string>> UpdateTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        Task<ApiResponse<string>> DeleteTwoEntitiesAsync<T1, T2>(T1 entity1, T2 entity2) where T1 : class where T2 : class;
        Task<ApiResponse<string>> DeleteTwoEntitiesByIdAsync<T1, T2>(int id1, int id2) where T1 : class where T2 : class;

        Task<ApiRespone_basic> AddRangeAsync(IEnumerable<T> entities);
        Task<ApiRespone_basic> UpdateRangeAsync(IEnumerable<T> entities);

        Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
        Task<bool> DeleteAllAsync();
        Task<bool> DeleteWhereAsync(Expression<Func<T, bool>> predicate);
        Task<ApiResponse<string>> UpdateRelatedEntitiesAsync<TParent, TChild>(
            TParent parent,
            List<TChild> childrenToAdd,
            List<TChild> childrenToUpdate,
            List<int> childIdsToDelete)
            where TParent : class
            where TChild : class;
        Task<bool> IsDuplicateAsync(Expression<Func<T, bool>> filter);
        ApiRespone_basic LoadComboBox(string? state);

        Task ProcessActionTypeListAsync<TDto>(List<TDto> dtos)
        where TDto : class, IActionDto;
    }
}
