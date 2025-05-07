using ERP.Entities;
using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Employee;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.APIs.Leaves.entity
{
    public class EmployeeLeaveBalance : BaseEntity
    {
        public int employeeId { get; set; }
        //
        public double totalDays { get; set; }
        public double usedDays { get; set; }
        public double remainingDays { get; set; }
        //
        public double usedDaysMonth { get; set; }
        public double remainingDaysMonth { get; set; }
        public double maxDayMonth { get; set; }
    }
}
