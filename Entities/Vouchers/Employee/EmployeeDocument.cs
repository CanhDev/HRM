using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Entities.Lists.Employee;

namespace ERP.Entities.Vouchers.Employee
{
    public class EmployeeDocument : BaseEntity
    {
        [Required]
        public string DocumentCode { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string DocumentName { get; set; }

        [StringLength(50)]
        public string DocumentType { get; set; }

        [StringLength(250)]
        public string FilePath { get; set; }



        [StringLength(250)]
        public string Notes { get; set; }

    }
}
