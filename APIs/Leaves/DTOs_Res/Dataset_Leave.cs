using ERP.DTO;
using System.ComponentModel.DataAnnotations;

namespace ERP.APIs.Leaves.DTOs
{
    public class Dataset_Leave
    {
        public required LeaveRequestDto ph { get; set; }
        public List<LeaveRequestDetaislDto>? ct { get; set; }
    }

    public class LeaveRequestDto
    {
        public int employeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }

        public double totalDays { get; set; }
        public string voucherCode { get; set; }
        public string? detailNote { get; set; }
        public int approvalStatus { get; set; } // trạng thái duyệt join với bảng sys_dmtt
        public int? departmentId { get; set; }
        public int status { get; set; } = 1;
    }

    public class LeaveRequestDetaislDto : IActionDto
    {
        public int leaveRequestId { get; set; }
        public int workCode { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public double totalDays { get; set; } // lấy end - start
        public int leaveType { get; set; }
        public string reason { get; set; }
        public short status { get; set; } = 1;
        public int id { get; set; }
        public string? actionType { get; set; }
    }
}
