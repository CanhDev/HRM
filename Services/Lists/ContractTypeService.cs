using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Lists.Contract;
using AutoMapper;

namespace ERP.Services.Lists
{

    public class ContractTypeService : BaseService<ContractType>
    {
        private readonly ILogger<ContractTypeService> _logger;
        public ContractTypeService(IBaseRepository<ContractType> repository, IMapper mapper,
            ApplicationDbContext context, ILogger<ContractTypeService> logger) : base(repository , mapper, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<ContractType>>> GetAllAsync(Expression<Func<ContractType, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<ContractType?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<ContractType>>> GetPagedListAsync(
            SearchRequest<ContractType> searchRequest, params Expression<Func<ContractType, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<ContractType>> AddAsync(ContractType entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<ContractType>> UpdateAsync(ContractType entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<ContractType>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }
}
