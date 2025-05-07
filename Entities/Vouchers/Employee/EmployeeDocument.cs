using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Entities.Lists.Employee;

namespace ERP.Entities.Vouchers.Employee
{
    public class EmployeeDocument : BaseEntity
    {
        public int employeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string documentName { get; set; }

        [StringLength(50)]
        public string documentType { get; set; }

        [StringLength(250)]
        public string filePath { get; set; }

        [StringLength(250)]
        public string notes { get; set; }

    }
}
