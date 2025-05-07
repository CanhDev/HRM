using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Entities.Vouchers;
using ERP.Entities.Lists.Contract;

namespace ERP.Entities.Lists.Employee
{
    public class Employee : BaseEntity
    {
        [Required]
        public required string employeeCode { get; set; }

        [StringLength(100)]
        public string? fullName { get; set; }
        public DateTime dob { get; set; }

        [StringLength(10)]
        public string? gender { get; set; }

        [StringLength(20)]
        public string? idCardNumber { get; set; }
        public DateTime? idCardIssueDate { get; set; }
        public string? idCardIssuePlace { get; set; }
        public string? taxCode { get; set; }
        public string? bankAccountNumber { get; set; }
        public string? bankName { get; set; }
        public string? bankBranch { get; set; }
        public DateTime? joinDate { get; set; }

        [StringLength(20)]
        public string? phone { get; set; }

        [StringLength(100)]
        public string? email { get; set; }

        [StringLength(255)]
        public string? address { get; set; }

        public string? imageUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal netSalary { get; set; } = 0;

        public int? departmentId { get; set; }

        public int? positionId { get; set; }

        public int? contractTypeId { get; set; }
    }
}
