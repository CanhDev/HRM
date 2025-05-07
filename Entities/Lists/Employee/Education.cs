using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Lists.Employee
{
    public class Education: BaseEntity
    {
        
        public int employeeId { get; set; }
        
        public string? degree { get; set; }
        public string? major { get; set; }
        public string? school { get; set; }  
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public float gpa { get; set; }
        public string? description { get; set; }
    }
}
