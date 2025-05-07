namespace ERP.Base_sys.Helpers
{
    public interface IEntityAuditHelper
    {
        void SetCreatedAuditInfo(object entity);
        void SetUpdatedAuditInfo(object entity);
    }
}
