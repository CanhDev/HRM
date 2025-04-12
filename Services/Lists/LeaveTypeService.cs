using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Lists.Leave;

namespace ERP.Services.Lists
{

    public class LeaveTypeService : BaseService<LeaveType>
    {
        private readonly ILogger<LeaveTypeService> _logger;
        public LeaveTypeService(IBaseRepository<LeaveType> repository, ApplicationDbContext context, ILogger<LeaveTypeService> logger) : base(repository, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<LeaveType>>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public override async Task<ApiResponse<LeaveType?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<LeaveType>>> GetPagedListAsync(
            SearchRequest<LeaveType> searchRequest, params Expression<Func<LeaveType, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<LeaveType>> AddAsync(LeaveType entity, Expression<Func<LeaveType, bool>> duplicateCheckFilter)
        {
            var res = await base.AddAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<LeaveType>> UpdateAsync(LeaveType entity, Expression<Func<LeaveType, bool>> duplicateCheckFilter)
        {
            var res = await base.UpdateAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<LeaveType>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }





}
