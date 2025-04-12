using System.Linq.Expressions;

namespace ERP.Base_sys.Repos
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<PagedResult<T>> GetPagedListAsync(SearchRequest<T> searchRequest,
                                           params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    }
}
