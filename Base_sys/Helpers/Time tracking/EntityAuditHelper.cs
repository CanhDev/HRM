using ERP.Entities;
using ERP.Entities._1_Configs;
using System.Security.Claims;

namespace ERP.Base_sys.Helpers
{
    public class EntityAuditHelper : IEntityAuditHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EntityAuditHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var userId =  _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId ?? string.Empty;
        }
        public void SetCreatedAuditInfo(object entity)
        {
            if (entity == null) return;

            var now = DateTime.UtcNow;
            string userId = GetUserId();

            if (entity is BaseEntity baseEntity)
            {
                baseEntity.createdAt = now;
                baseEntity.updateAt = now;
                baseEntity.createBy = userId;
                baseEntity.updateBy = userId;
            }
            else if (entity is ApplicationUser appUser)
            {
                appUser.createdAt = now;
                appUser.updateAt = now;
                appUser.createBy = userId;
                appUser.updateBy = userId;
            }
        }

        public void SetUpdatedAuditInfo(object entity)
        {
            if (entity == null) return;

            var now = DateTime.UtcNow;
            var userId = GetUserId();

            if (entity is BaseEntity auditEntity)
            {
               if(auditEntity.updateAt != null) auditEntity.updateAt = now;
                auditEntity.updateBy = userId;
            }
            else if (entity is ApplicationUser appUser)
            {
                appUser.updateAt = now;
                appUser.updateBy = userId;
            }
        }
    }

}
