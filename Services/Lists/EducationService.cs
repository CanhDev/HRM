using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Lists.Employee;
using AutoMapper;

namespace ERP.Services.Lists
{

    public class EducationService : BaseService<Education>
    {
        private readonly ILogger<EducationService> _logger;
        public EducationService(IBaseRepository<Education> repository, IMapper mapper, ApplicationDbContext context, ILogger<EducationService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<Education>>> GetAllAsync(Expression<Func<Education, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<Education?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<Education>>> GetPagedListAsync(
            SearchRequest<Education> searchRequest, params Expression<Func<Education, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<Education>> AddAsync(Education entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<Education>> UpdateAsync(Education entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<Education>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }





}
