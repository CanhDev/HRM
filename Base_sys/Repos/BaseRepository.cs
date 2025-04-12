using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using ERP.Entities;

namespace ERP.Base_sys.Repos
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<PagedResult<T>> GetPagedListAsync(SearchRequest<T> searchRequest,
                                         params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            // Bao gồm các thuộc tính cần thiết
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Xử lý tìm kiếm toàn cục
            if (!string.IsNullOrEmpty(searchRequest.GlobalSearch))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression? combinedExpression = null;

                foreach (var property in typeof(T).GetProperties())
                {
                    // Chỉ xử lý các thuộc tính có thể đọc
                    if (!property.CanRead)
                        continue;

                    // Bỏ qua các thuộc tính phức tạp (navigation properties)
                    if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                        continue;

                    var member = Expression.Property(parameter, property);

                    try
                    {
                        // Xử lý các thuộc tính kiểu chuỗi
                        if (property.PropertyType == typeof(string))
                        {
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            var valueExpression = Expression.Constant(searchRequest.GlobalSearch);

                            // Xử lý trường hợp thuộc tính có thể null
                            var notNullCheck = Expression.NotEqual(member, Expression.Constant(null));
                            var containsCall = Expression.Call(member, containsMethod!, valueExpression);
                            var safePredicate = Expression.AndAlso(notNullCheck, containsCall);

                            combinedExpression = combinedExpression == null
                                ? safePredicate
                                : Expression.OrElse(combinedExpression, safePredicate);
                        }
                        // Xử lý các kiểu dữ liệu số nguyên
                        else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                        {
                            if (int.TryParse(searchRequest.GlobalSearch, out var intValue))
                            {
                                Expression predicate;
                                if (property.PropertyType == typeof(int?))
                                {
                                    var valueExpression = Expression.Constant(intValue, typeof(int?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                                else
                                {
                                    var valueExpression = Expression.Constant(intValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }

                                combinedExpression = combinedExpression == null
                                    ? predicate
                                    : Expression.OrElse(combinedExpression, predicate);
                            }
                        }
                        // Xử lý các kiểu dữ liệu số thực
                        else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?) ||
                                 property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
                        {
                            if (decimal.TryParse(searchRequest.GlobalSearch, out var decimalValue))
                            {
                                Expression predicate;

                                if (property.PropertyType == typeof(decimal))
                                {
                                    var valueExpression = Expression.Constant(decimalValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                                else if (property.PropertyType == typeof(decimal?))
                                {
                                    var valueExpression = Expression.Constant(decimalValue, typeof(decimal?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                                else if (property.PropertyType == typeof(double))
                                {
                                    var valueExpression = Expression.Constant((double)decimalValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                                else // double?
                                {
                                    var valueExpression = Expression.Constant((double)decimalValue, typeof(double?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }

                                combinedExpression = combinedExpression == null
                                    ? predicate
                                    : Expression.OrElse(combinedExpression, predicate);
                            }
                        }
                        // Xử lý kiểu dữ liệu ngày tháng
                        else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                        {
                            if (DateTime.TryParse(searchRequest.GlobalSearch, out var dateValue))
                            {
                                Expression predicate;

                                if (property.PropertyType == typeof(DateTime))
                                {
                                    var valueExpression = Expression.Constant(dateValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                                else // DateTime?
                                {
                                    var valueExpression = Expression.Constant(dateValue, typeof(DateTime?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }

                                combinedExpression = combinedExpression == null
                                    ? predicate
                                    : Expression.OrElse(combinedExpression, predicate);
                            }
                        }
                    }
                    catch
                    {
                        // Bỏ qua các lỗi khi xử lý thuộc tính
                        continue;
                    }
                }

                // Thêm điều kiện tìm kiếm vào câu truy vấn
                if (combinedExpression != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                    query = query.Where(lambda);
                }
            }

            // Xử lý các bộ lọc ngày
            if (searchRequest.DateFilters != null && searchRequest.DateFilters.Any())
            {
                foreach (var filter in searchRequest.DateFilters)
                {
                    var property = typeof(T).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)))
                    {
                        var parameter = Expression.Parameter(typeof(T), "x");
                        var member = Expression.Property(parameter, property);

                        Expression? datePredicate = null;

                        // Kiểm tra ngày bắt đầu
                        if (filter.Value.From.HasValue)
                        {
                            Expression greaterThanOrEqual;
                            if (property.PropertyType == typeof(DateTime))
                            {
                                var fromConstant = Expression.Constant(filter.Value.From.Value);
                                greaterThanOrEqual = Expression.GreaterThanOrEqual(member, fromConstant);
                            }
                            else // DateTime?
                            {
                                var fromConstant = Expression.Constant(filter.Value.From.Value, typeof(DateTime?));
                                var notNullCheck = Expression.NotEqual(member, Expression.Constant(null, typeof(DateTime?)));
                                var comparison = Expression.GreaterThanOrEqual(member, fromConstant);
                                greaterThanOrEqual = Expression.AndAlso(notNullCheck, comparison);
                            }

                            datePredicate = greaterThanOrEqual;
                        }

                        // Kiểm tra ngày kết thúc
                        if (filter.Value.To.HasValue)
                        {
                            Expression lessThanOrEqual;
                            if (property.PropertyType == typeof(DateTime))
                            {
                                var toConstant = Expression.Constant(filter.Value.To.Value);
                                lessThanOrEqual = Expression.LessThanOrEqual(member, toConstant);
                            }
                            else // DateTime?
                            {
                                var toConstant = Expression.Constant(filter.Value.To.Value, typeof(DateTime?));
                                var notNullCheck = Expression.NotEqual(member, Expression.Constant(null, typeof(DateTime?)));
                                var comparison = Expression.LessThanOrEqual(member, toConstant);
                                lessThanOrEqual = Expression.AndAlso(notNullCheck, comparison);
                            }

                            datePredicate = datePredicate == null
                                ? lessThanOrEqual
                                : Expression.AndAlso(datePredicate, lessThanOrEqual);
                        }

                        if (datePredicate != null)
                        {
                            var lambda = Expression.Lambda<Func<T, bool>>(datePredicate, parameter);
                            query = query.Where(lambda);
                        }
                    }
                }
            }

            // Xử lý các bộ lọc cột
            if (searchRequest.ColumnFilters != null && searchRequest.ColumnFilters.Any())
            {
                foreach (var filter in searchRequest.ColumnFilters)
                {
                    var property = typeof(T).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        var parameter = Expression.Parameter(typeof(T), "x");
                        var member = Expression.Property(parameter, property);

                        try
                        {
                            Expression? predicate = null;

                            // Xử lý các kiểu dữ liệu cơ bản
                            if (property.PropertyType == typeof(string))
                            {
                                var valueExpression = Expression.Constant(filter.Value);

                                // Xử lý trường hợp thuộc tính có thể null
                                var notNullCheck = Expression.NotEqual(member, Expression.Constant(null));
                                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                var containsCall = Expression.Call(member, containsMethod!, valueExpression);
                                predicate = Expression.AndAlso(notNullCheck, containsCall);
                            }
                            else if (property.PropertyType == typeof(int))
                            {
                                if (int.TryParse(filter.Value, out var intValue))
                                {
                                    var valueExpression = Expression.Constant(intValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(int?))
                            {
                                if (int.TryParse(filter.Value, out var intValue))
                                {
                                    var valueExpression = Expression.Constant(intValue, typeof(int?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(decimal))
                            {
                                if (decimal.TryParse(filter.Value, out var decimalValue))
                                {
                                    var valueExpression = Expression.Constant(decimalValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(decimal?))
                            {
                                if (decimal.TryParse(filter.Value, out var decimalValue))
                                {
                                    var valueExpression = Expression.Constant(decimalValue, typeof(decimal?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(double))
                            {
                                if (double.TryParse(filter.Value, out var doubleValue))
                                {
                                    var valueExpression = Expression.Constant(doubleValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(double?))
                            {
                                if (double.TryParse(filter.Value, out var doubleValue))
                                {
                                    var valueExpression = Expression.Constant(doubleValue, typeof(double?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(DateTime))
                            {
                                if (DateTime.TryParse(filter.Value, out var dateValue))
                                {
                                    var valueExpression = Expression.Constant(dateValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(DateTime?))
                            {
                                if (DateTime.TryParse(filter.Value, out var dateValue))
                                {
                                    var valueExpression = Expression.Constant(dateValue, typeof(DateTime?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(bool))
                            {
                                if (bool.TryParse(filter.Value, out var boolValue))
                                {
                                    var valueExpression = Expression.Constant(boolValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType == typeof(bool?))
                            {
                                if (bool.TryParse(filter.Value, out var boolValue))
                                {
                                    var valueExpression = Expression.Constant(boolValue, typeof(bool?));
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }
                            else if (property.PropertyType.IsEnum)
                            {
                                if (int.TryParse(filter.Value, out var intValue) &&
                                    Enum.IsDefined(property.PropertyType, intValue))
                                {
                                    var enumValue = Enum.ToObject(property.PropertyType, intValue);
                                    var valueExpression = Expression.Constant(enumValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                                else if (Enum.TryParse(property.PropertyType, filter.Value, out var enumValue))
                                {
                                    var valueExpression = Expression.Constant(enumValue);
                                    predicate = Expression.Equal(member, valueExpression);
                                }
                            }

                            if (predicate != null)
                            {
                                var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
                                query = query.Where(lambda);
                            }
                        }
                        catch
                        {
                            // Bỏ qua nếu có lỗi khi xử lý biểu thức
                            continue;
                        }
                    }
                }
            }

            // Xử lý sắp xếp - Cải thiện phần này để tránh lỗi
            if (!string.IsNullOrEmpty(searchRequest.SortBy))
            {
                var property = typeof(T).GetProperty(searchRequest.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null)
                {
                    var parameter = Expression.Parameter(typeof(T), "e");
                    var propertyAccess = Expression.Property(parameter, property);

                    // Tạo một loại chung cho biểu thức lambda
                    var lambdaType = typeof(Func<,>).MakeGenericType(typeof(T), property.PropertyType);
                    var lambda = Expression.Lambda(lambdaType, propertyAccess, parameter);

                    // Xác định phương thức OrderBy hoặc OrderByDescending
                    string methodName = searchRequest.SortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

                    // Tạo biểu thức gọi phương thức
                    var methodCall = Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new Type[] { typeof(T), property.PropertyType },
                        query.Expression,
                        Expression.Quote(lambda)
                    );

                    // Tạo truy vấn mới với sắp xếp
                    query = query.Provider.CreateQuery<T>(methodCall);
                }
                else
                {
                    // Nếu không tìm thấy thuộc tính, thử sắp xếp theo Id (thuộc tính chung)
                    try
                    {
                        var idProperty = typeof(T).GetProperty("Id");
                        if (idProperty != null)
                        {
                            query = searchRequest.SortOrder.ToLower() == "desc"
                                ? query.OrderByDescending(e => EF.Property<object>(e, "Id"))
                                : query.OrderBy(e => EF.Property<object>(e, "Id"));
                        }
                    }
                    catch
                    {
                        // Bỏ qua sắp xếp nếu không thể sắp xếp theo Id
                    }
                }
            }

            // Tính tổng số bản ghi trước khi áp dụng phân trang
            int totalRecords = await query.CountAsync();

            // Áp dụng phân trang
            var pageNumber = searchRequest.Page > 0 ? searchRequest.Page : 1;
            var pageSize = searchRequest.PageSize > 0 ? searchRequest.PageSize : 10;

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Lấy dữ liệu
            var items = await query.ToListAsync();

            // Trả về kết quả phân trang
            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalRecords,
                Page = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }
    }
}
