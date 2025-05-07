using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Lists.Employee;
using AutoMapper;

namespace ERP.Services.Lists
{

    public class EmergencyContactService : BaseService<EmergencyContact>
    {
        private readonly ILogger<EmergencyContactService> _logger;
        public EmergencyContactService(IBaseRepository<EmergencyContact> repository, IMapper mapper, ApplicationDbContext context, ILogger<EmergencyContactService> logger) 
            : base(repository, mapper, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<EmergencyContact>>> GetAllAsync(Expression<Func<EmergencyContact, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
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
        public override async Task<ApiResponse<EmergencyContact>> AddAsync(EmergencyContact entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<EmergencyContact>> UpdateAsync(EmergencyContact entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<EmergencyContact>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }





}
