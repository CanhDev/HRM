using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Employee;
using ERP.Entities.Lists.Leave;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Vouchers.Leave
{
    public class EmployeeLeaveBalance : BaseVoucherEntity
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }

        public int year { get; set; }
        public decimal TotalDays { get; set; }
        public decimal UsedDays { get; set; }
        public decimal RemainingDays { get; set; }
        public decimal CarriedDays { get; set; }
        public DateTime ExpiryDate { get; set; }



        

    }
}
