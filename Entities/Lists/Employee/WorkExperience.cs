using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Lists.Employee
{
    public class WorkExperience : BaseEntity
    {
        
        public int employeeId { get; set; }
        public string? companyName { get; set; }
        public string? position { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string? description { get; set; }
    }
}
