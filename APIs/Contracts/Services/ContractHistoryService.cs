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

    public class ContractHistoryService : BaseService<ContractHistory>
    {
        private readonly ILogger<ContractHistoryService> _logger;
        private readonly FileHelper _fileHelper;
        private readonly IMapper _mapper;
        public ContractHistoryService(IBaseRepository<ContractHistory> repository,
         ApplicationDbContext context,
         FileHelper fileHelper,
          IMapper mapper,
        ILogger<ContractHistoryService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _mapper = mapper;
        }


        public override async Task<ApiResponse<List<ContractHistory>>> GetAllAsync(Expression<Func<ContractHistory, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<ContractHistory?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<ContractHistory>>> GetPagedListAsync(
            SearchRequest<ContractHistory> searchRequest, params Expression<Func<ContractHistory, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<ContractHistory>> AddAsync(ContractHistory entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<ContractHistory>> UpdateAsync(ContractHistory entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<ContractHistory>> DeleteAsync(int id)
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
