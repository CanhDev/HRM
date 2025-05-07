using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using AutoMapper;
using ERP.APIs.Leaves.entity;

namespace ERP.APIs.Leaves.Services
{

    public class LeaveTypeService : BaseService<LeaveType>
    {
        private readonly ILogger<LeaveTypeService> _logger;
        public LeaveTypeService(IBaseRepository<LeaveType> repository, IMapper mapper, ApplicationDbContext context, ILogger<LeaveTypeService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<LeaveType>>> GetAllAsync(Expression<Func<LeaveType, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
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
        public override async Task<ApiResponse<LeaveType>> AddAsync(LeaveType entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<LeaveType>> UpdateAsync(LeaveType entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<LeaveType>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }





}
