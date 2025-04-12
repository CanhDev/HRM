using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Employee;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Vouchers.Leave
{
    public class LeaveBalanceHistory : BaseVoucherEntity
    {
        public int ChangeBy { get; set; }
        public string BalanceID { get; set; }

        public string ChangeType { get; set; }
        public DateTime ChangeDate { get; set; }
        public decimal DaysChanged { get; set; }
        public decimal OldBalance { get; set; }
        public decimal NewBalance { get; set; }
        public string Reason { get; set; }



        
    }
}
