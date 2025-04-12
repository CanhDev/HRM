using ERP.Entities._0_Systems;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Entities.Lists.Employee;
using ERP.Entities.Lists.Leave;

namespace ERP.Entities.Vouchers.Leave
{
    public class LeaveRequest : BaseVoucherEntity
    {

        [Required]
        [StringLength(50)]
        public string LeaveRequestCode { get; set; }

        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public double TotalDays { get; set; }

        [StringLength(250)]
        public string Reason { get; set; }


        


    }
}
