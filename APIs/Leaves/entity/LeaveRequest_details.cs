using ERP.Entities;

namespace ERP.APIs.Leaves.entity
{
    public class LeaveRequest_details : BaseEntity
    {
        public int leaveRequestId { get; set; }
        public int workCode { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal totalDays { get; set; } // lấy end - start
        public int leaveType { get; set; }
        public string reason { get; set; }
    }
}
