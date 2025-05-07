using System.Linq.Expressions;

namespace ERP.Base_sys.Repos
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
        Task<PagedResult<T>> GetPagedListAsync(SearchRequest<T> searchRequest,
                                           params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
        Task<bool> DeleteAllAsync();
        Task<bool> DeleteWhereAsync(Expression<Func<T, bool>> predicate);

        //
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities);
    }
}
