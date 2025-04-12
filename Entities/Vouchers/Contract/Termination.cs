using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Employee;

namespace ERP.Entities.Vouchers.Contract
{
    public class Termination : BaseVoucherEntity
    {


        [Required]
        [StringLength(50)]
        public string TerminationCode { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TerminationDate { get; set; }

        [Required]
        [StringLength(50)]
        public string TerminationType { get; set; }

        [StringLength(250)]
        public string Reason { get; set; }

        [StringLength(250)]
        public string Notes { get; set; }

    }
}
