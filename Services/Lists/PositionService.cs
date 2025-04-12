using ERP.Base_sys;
using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Entities;
using ERP.Entities.Lists.Employee;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP.Services.Lists
{
    public class PositionService : BaseService<Position>
    {
        private readonly ILogger<PositionService> _logger;
        public PositionService(IBaseRepository<Position> repository, ApplicationDbContext context, ILogger<PositionService> logger) : base(repository, context)
        {
            _logger = logger;
        }


        public override async Task<ApiResponse<List<Position>>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }
        
        public override async Task<ApiResponse<Position?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<Position>>> GetPagedListAsync(
            SearchRequest<Position> searchRequest,params Expression<Func<Position, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<Position>> AddAsync(Position entity, Expression<Func<Position, bool>> duplicateCheckFilter)
        {
            var res = await base.AddAsync(entity, duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<Position>> UpdateAsync(Position entity, Expression<Func<Position, bool>> duplicateCheckFilter)
        {
            var res = await base.UpdateAsync(entity,duplicateCheckFilter);
            return res;
        }
        public override async Task<ApiResponse<Position>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
    }
}
