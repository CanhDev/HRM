using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Lists.Employee
{
    public class Position : BaseEntity
    {
        [Required]
        public string PositionCode { get; set; }

        [Required, StringLength(100)]
        public string PositionName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
