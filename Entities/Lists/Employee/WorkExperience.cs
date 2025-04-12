using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Lists.Employee
{
    public class WorkExperience : BaseEntity
    {
        
        public int EmployeeId { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
    }
}
