using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Lists.Employee;

namespace ERP.Services.Lists
{

    public class EmergencyContactService : BaseService<EmergencyContact>
    {
        private readonly ILogger<EmergencyContactService> _logger;
        public EmergencyContactService(IBaseRepository<EmergencyContact> repository, ApplicationDbContext context, ILogger<EmergencyContactService> logger) : base(repository, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<EmergencyContact>>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public override async Task<ApiResponse<EmergencyContact?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<EmergencyContact>>> GetPagedListAsync(
            SearchRequest<EmergencyContact> searchRequest, params Expression<Func<EmergencyContact, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<EmergencyContact>> AddAsync(EmergencyContact entity, Expression<Func<EmergencyContact, bool>> duplicateCheckFilter)
        {
            var res = await base.AddAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<EmergencyContact>> UpdateAsync(EmergencyContact entity, Expression<Func<EmergencyContact, bool>> duplicateCheckFilter)
        {
            var res = await base.UpdateAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<EmergencyContact>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }





}
