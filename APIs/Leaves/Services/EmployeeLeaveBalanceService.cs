using AutoMapper;
using ERP.Base_sys.Helpers;
using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using System.Linq.Expressions;
using ERP.APIs.Leaves.entity;
using ERP.Entities;

namespace ERP.APIs.Leaves.Services
{

    public class EmployeeLeaveBalanceService : BaseService<EmployeeLeaveBalance>
    {
        private readonly ILogger<EmployeeLeaveBalanceService> _logger;
        private readonly FileHelper _fileHelper;
        private readonly IMapper _mapper;
        public EmployeeLeaveBalanceService(IBaseRepository<EmployeeLeaveBalance> repository,
         ApplicationDbContext context,
         FileHelper fileHelper,
          IMapper mapper,
        ILogger<EmployeeLeaveBalanceService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _mapper = mapper;
        }


        public override async Task<ApiResponse<List<EmployeeLeaveBalance>>> GetAllAsync(Expression<Func<EmployeeLeaveBalance, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<EmployeeLeaveBalance?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<EmployeeLeaveBalance>>> GetPagedListAsync(
            SearchRequest<EmployeeLeaveBalance> searchRequest, params Expression<Func<EmployeeLeaveBalance, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<EmployeeLeaveBalance>> AddAsync(EmployeeLeaveBalance entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<EmployeeLeaveBalance>> UpdateAsync(EmployeeLeaveBalance entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<EmployeeLeaveBalance>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
        public override async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
        {
            return await base.DeleteRangeAsync(ids);
        }
        public override async Task<bool> DeleteAllAsync()
        {
            return await base.DeleteAllAsync();
        }
        
    }





}
