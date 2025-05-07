using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Lists.Employee
{
    public class Department : BaseEntity
    {
        [Required]
        public string departmentCode { get; set; }

        [Required, StringLength(100)]
        public string departmentName { get; set; }

        public int? managerID { get; set; }

    }
}
