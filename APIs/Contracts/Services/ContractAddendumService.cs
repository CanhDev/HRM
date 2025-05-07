using AutoMapper;
using ERP.Base_sys.Helpers;
using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using System.Linq.Expressions;
using ERP.Entities;
using ERP.APIs.Contracts.Model.Contract;

namespace ERP.APIs.Contracts.Services
{

    public class ContractAddendumService : BaseService<ContractAddendum>
    {
        private readonly ILogger<ContractAddendumService> _logger;
        private readonly FileHelper _fileHelper;
        private readonly IMapper _mapper;
        public ContractAddendumService(IBaseRepository<ContractAddendum> repository,
         ApplicationDbContext context,
         FileHelper fileHelper,
          IMapper mapper,
        ILogger<ContractAddendumService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _mapper = mapper;
        }


        public override async Task<ApiResponse<List<ContractAddendum>>> GetAllAsync(Expression<Func<ContractAddendum, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<ContractAddendum?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<ContractAddendum>>> GetPagedListAsync(
            SearchRequest<ContractAddendum> searchRequest, params Expression<Func<ContractAddendum, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<ContractAddendum>> AddAsync(ContractAddendum entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<ContractAddendum>> UpdateAsync(ContractAddendum entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<ContractAddendum>> DeleteAsync(int id)
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
