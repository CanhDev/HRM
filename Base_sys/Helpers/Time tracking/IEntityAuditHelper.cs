namespace ERP.Base_sys.Helpers
{
    public interface IEntityAuditHelper
    {
        void SetCreatedAuditInfo(object entity, string userId);
        void SetUpdatedAuditInfo(object entity, string userId);
    }
}
