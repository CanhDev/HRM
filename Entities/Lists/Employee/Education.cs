using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Lists.Employee
{
    public class Education: BaseEntity
    {
        
        public int EmployeeId { get; set; }
        
        public string Degree { get; set; }
        public string Major { get; set; }
        public string School { get; set; }  
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float GPA { get; set; }
        public string Description { get; set; }
    }
}
