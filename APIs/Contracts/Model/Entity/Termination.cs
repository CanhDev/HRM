using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Employee;
using ERP.Entities;

namespace ERP.APIs.Contracts.Model.Contract
{
    public class Termination : BaseEntity
    {
        public int contractId { get; set; }

        [Required]
        [StringLength(50)]
        public string terminationCode { get; set; }

        public int employeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime terminationDate { get; set; }

        [Required]
        [StringLength(50)]
        public string terminationType { get; set; }

        [StringLength(250)]
        public string reason { get; set; }

        [StringLength(250)]
        public string notes { get; set; }
    }
}
