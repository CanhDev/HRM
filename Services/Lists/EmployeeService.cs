using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Lists.Employee;

namespace ERP.Services.Lists
{

    public class EmployeeService : BaseService<Employee>
    {
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(IBaseRepository<Employee> repository, ApplicationDbContext context, ILogger<EmployeeService> logger) : base(repository, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<Employee>>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public override async Task<ApiResponse<Employee?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<Employee>>> GetPagedListAsync(
            SearchRequest<Employee> searchRequest, params Expression<Func<Employee, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<Employee>> AddAsync(Employee entity, Expression<Func<Employee, bool>> duplicateCheckFilter)
        {
            var res = await base.AddAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<Employee>> UpdateAsync(Employee entity, Expression<Func<Employee, bool>> duplicateCheckFilter)
        {
            var res = await base.UpdateAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<Employee>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }





}
