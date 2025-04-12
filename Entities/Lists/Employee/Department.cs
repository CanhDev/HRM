using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Lists.Employee
{
    public class Department : BaseEntity
    {
        [Required]
        public string DepartmentCode { get; set; }

        [Required, StringLength(100)]
        public string DepartmentName { get; set; }

        public int? ManagerID { get; set; }

    }
}
