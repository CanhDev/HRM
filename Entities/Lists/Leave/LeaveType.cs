using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Lists.Leave
{
    public class LeaveType : BaseEntity
    {


        [Required, StringLength(100)]
        public string LeaveTypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        public short IsPaid { get; set; }
        public short CarryForward { get; set; }
        public short MaxCarryForwardDays { get; set; }

        public int MaxDaysAllowed { get; set; }
    }
}
