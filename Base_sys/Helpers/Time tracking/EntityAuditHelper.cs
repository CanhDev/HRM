using ERP.Entities;

namespace ERP.Base_sys.Helpers
{
    public class EntityAuditHelper : IEntityAuditHelper
    {
        public void SetCreatedAuditInfo(object entity, string userId)
        {
            if (entity == null) return;

            var now = DateTime.UtcNow;

            if (entity is BaseEntity auditEntity)
            {
                auditEntity.createdAt = now;
                auditEntity.updateAt = now;
                auditEntity.createBy = userId;
                auditEntity.updateBy = userId;
            }
        }

        public void SetUpdatedAuditInfo(object entity, string userId)
        {
            if (entity == null) return;

            var now = DateTime.UtcNow;

            if (entity is BaseEntity auditEntity)
            {
                auditEntity.updateAt = now;
                auditEntity.updateBy = userId;
            }
        }
    }

}
