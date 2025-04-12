using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Lists.Employee;

namespace ERP.Services.Lists
{

    public class WorkExperienceService : BaseService<WorkExperience>
    {
        private readonly ILogger<WorkExperienceService> _logger;
        public WorkExperienceService(IBaseRepository<WorkExperience> repository, ApplicationDbContext context, ILogger<WorkExperienceService> logger) : base(repository, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<WorkExperience>>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public override async Task<ApiResponse<WorkExperience?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<WorkExperience>>> GetPagedListAsync(
            SearchRequest<WorkExperience> searchRequest, params Expression<Func<WorkExperience, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<WorkExperience>> AddAsync(WorkExperience entity, Expression<Func<WorkExperience, bool>> duplicateCheckFilter)
        {
            var res = await base.AddAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<WorkExperience>> UpdateAsync(WorkExperience entity, Expression<Func<WorkExperience, bool>> duplicateCheckFilter)
        {
            var res = await base.UpdateAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<WorkExperience>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }





}
