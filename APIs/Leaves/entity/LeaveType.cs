using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Entities;

namespace ERP.APIs.Leaves.entity
{
    public class LeaveType : BaseEntity
    {
        [Required]
        public required string leaveTypeCode { get; set; }
        [Required, StringLength(100)]
        public string leaveTypeName { get; set; }

        [StringLength(255)]
        public string description { get; set; }

        public short isPaid { get; set; }
        public int maxDaysAllowed { get; set; }
    }
}
