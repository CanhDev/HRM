using ERP.Entities._0_Systems;
using System.ComponentModel.DataAnnotations;

namespace ERP.APIs.Leaves.entity
{
    public class LeaveRequest : BaseVoucherEntity
    {
        public int employeeId { get; set; }

        public DateTime createDate { get; set; }

        public double totalDays { get; set; }
    }
}
