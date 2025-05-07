using ERP.Entities;
using ERP.Entities._0_Systems;
using System.ComponentModel.DataAnnotations;

namespace ERP.APIs.Leaves.DTOs_Res
{
    public class DataResponse_Leave
    {
        public LeaveRequestRes ph { get; set; }
        public List<LeaveRequestDetailsRes>? ct { get; set; }
        public List<LeaveBalanceResSub>? ctBalance { get; set; }
    }

    public class LeaveRequestRes : BaseVoucherEntity
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

    public class LeaveBalanceResSub
    {
        public double usedDaysMonth { get; set; }
        public double remainingDaysMonth { get; set; }
        public double maxDayMonth { get; set; }
    }

    public class LeaveBalanceRes : BaseEntity
    {
        public int employeeId { get; set; }
        public double totalDays { get; set; }
        public double usedDays { get; set; }
        public double remainingDays { get; set; }
        public double remainingDaysMonth { get; set; }
        public double maxDayMonth { get; set; }
    }

    public class LeaveRequestDetailsRes
    {
        public int leaveRequestId { get; set; }
        public int workCode { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public double totalDays { get; set; }
        public int leaveType { get; set; }
        public string reason { get; set; }
        public short status { get; set; }
        public int id { get; set; }
    }
}
