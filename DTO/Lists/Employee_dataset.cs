using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.DTO.Lists
{
    public class EmployeeCreateRequest
    {
        public EmployeeDTO employeeDTO { get; set; }
        public string? emergencyContactsJson { get; set; }
        public string? educationListJson { get; set; }
        public string? workExperienceListJson { get; set; }
        public IFormFile? imageFile { get; set; }
    }
    public class Employee_dataset
    {
        public required EmployeeDTO employeeDTO { get; set; }
        public ICollection<EmergencyContactDTO>? emergencyContactList { get; set; } = new List<EmergencyContactDTO>();
        public ICollection<EducationDTO>? educationList { get; set; } = new List<EducationDTO>();
        public ICollection<WorkExperienceDTO>? workExperienceList { get; set; } = new List<WorkExperienceDTO>();

    }
    public class EmployeeDTO
    {
        public string? employeeCode { get; set; }

        [Required, StringLength(100)]
        public string fullName { get; set; }

        public DateTime? dob { get; set; } = DateTime.MinValue;

        [StringLength(10)]
        public string ? gender { get; set; }

        [StringLength(20)]
        public string? idCardNumber { get; set; }

        public DateTime? idCardIssueDate { get; set; } = DateTime.MinValue;

        public string? idCardIssuePlace { get; set; }

        public string? taxCode { get; set; }

        public string? bankAccountNumber { get; set; }

        public string? bankName { get; set; }

        public string? bankBranch { get; set; }

        public DateTime? joinDate { get; set; } = DateTime.MinValue;

        [StringLength(20)]
        public string? phone { get; set; }

        [StringLength(100)]
        public string? email { get; set; }

        [StringLength(255)]
        public string? address { get; set; }

        public decimal? netSalary { get; set; } = 0;

        public int? departmentId { get; set; }

        public int? positionId { get; set; }

        public int? contractTypeId { get; set; }

        public string? imageUrl { get; set; }

        public IFormFile? imageFile { get; set; }

        public int status { get; set; } = 1;
    }

    public class EmergencyContactDTO : IActionDto
    {
        public int id { get; set; }
        public int? employeeId { get; set; }

        public string? relationship { get; set; }
        public string? fullName { get; set; }
        public string? phoneNumber { get; set; }
        public string? address { get; set; }
        public string? actionType { get; set; }

        public int status { get; set; } = 1;
    }

    public class EducationDTO : IActionDto
    {
        public int id { get; set; }
        public string? actionType { get; set; }
        public int? employeeId { get; set; }

        public string? degree { get; set; }
        public string? major { get; set; }
        public string? school { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public float? gpa { get; set; }

        public string? description { get; set; }

        public int status { get; set; } = 1;
    }

    public class WorkExperienceDTO : IActionDto
    {
        public int id { get; set; }
        public int? employeeId { get; set; }

        public string? companyName { get; set; }
        public string? position { get; set; }

        public DateTime? startDate { get; set; }   
        public DateTime? endDate { get; set; }

        public string? description { get; set; }
        public string? actionType { get; set; }

        public int status { get; set; } = 1;
    }

    public class EmployeeDocumentDTO : IActionDto
    {
        public int id { get; set; }
        public int employeeId { get; set; }

        [StringLength(100)]
        public string? documentName { get; set; }

        [StringLength(50)]
        public string? documentType { get; set; }

        [StringLength(250)]
        public string? filePath { get; set; }


        [StringLength(250)]
        public string? notes { get; set; }
        public IFormFile? docfile { get; set; }
        public int status { get; set; } = 1;
        public string? actionType { get; set; }
    }
}
