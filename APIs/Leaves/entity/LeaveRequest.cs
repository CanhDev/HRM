using ERP.Entities._0_Systems;
using System.ComponentModel.DataAnnotations;

namespace ERP.APIs.Leaves.entity
{
    public class LeaveRequest : BaseVoucherEntity
    {
        public int employeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }

        public double totalDays { get; set; }
    }
}
