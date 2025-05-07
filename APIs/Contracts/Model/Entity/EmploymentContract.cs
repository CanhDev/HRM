using ERP.Entities;
using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Contract;
using ERP.Entities.Lists.Employee;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.APIs.Contracts.Model.Contract
{
    public class EmploymentContract : BaseEntity
    {
        public int employeeId { get; set; }
        public int? departmentId { get; set; }

        public int? positionId { get; set; }

        public string? contractCode { get; set; }
        public int contractTypeId { get; set; }
        public decimal salary { get; set; }
        [StringLength(250)]
        public string? notes { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? signedDate { get; set; }
        public string? workingTime { get; set; }
        public string? jobDescription { get; set; }
        public string? benefits { get; set; }
        public string? filePath { get; set; }
        public string? terms { get; set; }
    }
}
