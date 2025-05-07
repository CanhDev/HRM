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

    public class LeaveRequest_detailsService : BaseService<LeaveRequest_details>
    {
        private readonly ILogger<LeaveRequest_detailsService> _logger;
        private readonly FileHelper _fileHelper;
        private readonly IMapper _mapper;
        public LeaveRequest_detailsService(IBaseRepository<LeaveRequest_details> repository,
         ApplicationDbContext context,
         FileHelper fileHelper,
          IMapper mapper,
        ILogger<LeaveRequest_detailsService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _mapper = mapper;
        }


        public override async Task<ApiResponse<List<LeaveRequest_details>>> GetAllAsync(Expression<Func<LeaveRequest_details, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<LeaveRequest_details?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<LeaveRequest_details>>> GetPagedListAsync(
            SearchRequest<LeaveRequest_details> searchRequest, params Expression<Func<LeaveRequest_details, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<LeaveRequest_details>> AddAsync(LeaveRequest_details entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<LeaveRequest_details>> UpdateAsync(LeaveRequest_details entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<LeaveRequest_details>> DeleteAsync(int id)
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
