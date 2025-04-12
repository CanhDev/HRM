using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.DTO.Lists
{

    public class Employee_dataset
    {
        public EmployeeDTO EmployeeDTO { get; set; }
        public ICollection<EmergencyContactDTO>? EmergencyContactDTOs { get; set; } = new List<EmergencyContactDTO>();
        public ICollection<EducationDTO>? EducationDTOs { get; set; } = new List<EducationDTO>();
        public ICollection<WorkExperienceDTO>? WorkExperienceDTOs { get; set; } = new List<WorkExperienceDTO>();

    }
    public class EmployeeDTO
    {
        public string EmployeeCode { get; set; }
        [Required, StringLength(100)]
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [StringLength(20)]
        public string IDCardNumber { get; set; }
        public DateTime IDCardIssueDate { get; set; }
        public string IDCardIssuePlacec { get; set; }
        public string TaxCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public DateTime JoinDate { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        public decimal NetSalary { get; set; }
        public int DepartmentID { get; set; }
        public int PositionID { get; set; }
        public int ContractTypeID { get; set; }
        public string imageUrl { get; set; }
        public IFormFile? imageFile { get; set; }
        
    }


    public class EmergencyContactDTO
    {
        public int EmployeeId { get; set; }

        public string Relationship { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
    public class EducationDTO
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
    public class WorkExperienceDTO
    {
        public int EmployeeId { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
    }
}
