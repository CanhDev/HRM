using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities.Lists.Employee
{
    public class EmergencyContact : BaseEntity
    {
       
        public int employeeId { get; set; }
        
        public string? relationship { get; set; }
        public string? fullName { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
    }
}
