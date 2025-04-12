using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Employee;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Vouchers.Contract
{
    public class ContractHistory : BaseVoucherEntity
    {
        public string ContractId { get; set; }
        
        public DateTime ChangeDate { get; set; }
        public int ChangeBy { get; set; }
        
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
