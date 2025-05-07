using System.ComponentModel.DataAnnotations;

namespace ERP.Data_res.Lists
{
    public class Employee_DataRes
    {
        public required Employee_res employeeRes { get; set; }
        public ICollection<EmergencyContact_res>? emergencyContactList { get; set; } = new List<EmergencyContact_res>();
        public ICollection<Education_res>? educationList { get; set; } = new List<Education_res>();
        public ICollection<WorkExperience_res>? workExperienceList { get; set; } = new List<WorkExperience_res>();
    }

    public class Employee_res
    {
        public int id { get; set; }
        public string employeeCode { get; set; }

        [Required, StringLength(100)]
        public string fullName { get; set; }
        public DateTime dob { get; set; }

        [StringLength(10)]
        public string gender { get; set; }

        [StringLength(20)]
        public string idCardNumber { get; set; }
        public DateTime idCardIssueDate { get; set; }
        public string idCardIssuePlace { get; set; }
        public string taxCode { get; set; }
        public string bankAccountNumber { get; set; }
        public string bankName { get; set; }
        public string bankBranch { get; set; }
        public DateTime joinDate { get; set; }

        [StringLength(20)]
        public string phone { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(255)]
        public string address { get; set; }
        public decimal netSalary { get; set; }
        public int departmentId { get; set; }
        public int positionId { get; set; }
        public int contractTypeId { get; set; }
        public string? imageUrl { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updateAt { get; set; }
        public string? createBy { get; set; }
        public string? updateBy { get; set; }
        public short? status { get; set; }
    }

    public class EmergencyContact_res
    {
        public int id { get; set; }
        public int employeeId { get; set; }

        public string relationship { get; set; }
        public string fullName { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public short? status { get; set; }
    }

    public class Education_res
    {
        public int id { get; set; }
        public int employeeId { get; set; }

        public string degree { get; set; }
        public string major { get; set; }
        public string school { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public float gpa { get; set; }
        public string description { get; set; }
        public short? status { get; set; }
    }

    public class WorkExperience_res
    {
        public int id { get; set; }
        public int employeeId { get; set; }
        public string companyName { get; set; }
        public string position { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string description { get; set; }
        public short? status { get; set; }
    }

    public class EmployeeDocument_res
    {
        public int id { get; set; }

        [StringLength(100)]
        public string documentName { get; set; }

        [StringLength(50)]
        public string documentType { get; set; }

        [StringLength(250)]
        public string filePath { get; set; }

        [StringLength(250)]
        public string notes { get; set; }

        public short? status { get; set; }
        public string? actionType { get; set; }
    }
}
