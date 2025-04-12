using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Entities.Vouchers;
using ERP.Entities.Lists.Contract;

namespace ERP.Entities.Lists.Employee
{
    public class Employee : BaseEntity
    {
        [Required]
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
        public string imageUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalary { get; set; }

        public int DepartmentID { get; set; }
        

        public int PositionID { get; set; }
        

        public int ContractTypeID { get; set; }
    

    }

}
