using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Lists.Employee
{
    public class Position : BaseEntity
    {
        [Required]
        public string positionCode { get; set; }

        [Required, StringLength(100)]
        public string positionName { get; set; }

        [StringLength(250)]
        public string description { get; set; }
    }
}
