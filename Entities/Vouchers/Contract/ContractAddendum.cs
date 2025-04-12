using ERP.Entities._0_Systems;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Vouchers.Contract
{
    public class ContractAddendum : BaseVoucherEntity
    {
        public string ContractId { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string AddendumContent { get; set; }
        public string FilePath { get; set; }

    }
}
