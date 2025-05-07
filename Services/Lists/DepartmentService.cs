using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ERP.Entities.Lists.Employee;
using ERP.Entities;
using AutoMapper;

namespace ERP.Services.Lists
{
    public class DepartmentService : BaseService<Department>
    {
        private readonly ILogger<DepartmentService> _logger;
        public DepartmentService(IBaseRepository<Department> repository, IMapper mapper, ApplicationDbContext context, ILogger<DepartmentService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<Department>>> GetAllAsync(Expression<Func<Department, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<Department?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<Department>>> GetPagedListAsync(
            SearchRequest<Department> searchRequest, params Expression<Func<Department, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<Department>> AddAsync(Department entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<Department>> UpdateAsync(Department entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<Department>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }
}
