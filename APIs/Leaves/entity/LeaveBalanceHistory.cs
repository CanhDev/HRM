using ERP.Entities;
using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Employee;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.APIs.Leaves.entity
{
    public class LeaveBalanceHistory : BaseEntity
    {
        public int? changeBy { get; set; }
        public int employeeId { get; set; }
        public DateTime changeDate { get; set; }
        public double daysChanged { get; set; }
        public string? reason { get; set; }
        public int mode { get; set; } // 
    }
}
