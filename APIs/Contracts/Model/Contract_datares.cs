using ERP.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.APIs.Contracts.Model
{
    public class Contract_datares
    {
        public ContractRes ContractRes { get; set; }
        public List<ContractAddendumRes>? ContractAddendumRes { get; set; }
        public List<ContractHistoryRes>? ContractHistoryRes { get; set; }
    }
    public class ContractRes : BaseEntity
    {
        public int EmployeeID { get; set; }
        public int? DepartmentID { get; set; }

        public int? PositionID { get; set; }

        public string? ContractCode { get; set; }
        public int? ContractTypeID { get; set; }
        public decimal Salary { get; set; }
        [StringLength(250)]
        public string? Notes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime SignedDate { get; set; }
        public string? WorkingTime { get; set; }
        public string? JobDescription { get; set; }
        public string? Benefits { get; set; }
        public string? FilePath { get; set; }
        public string? Terms { get; set; }
    }
    public class ContractAddendumRes : BaseEntity
    {
        public int ContractId { get; set; }
        public string? Title { get; set; }
        public string? ChangeField { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? AddendumContent { get; set; }
    }
    public class ContractHistoryRes : BaseEntity
    {
        public int ContractId { get; set; }

        public DateTime ChangeDate { get; set; }
        public int ChangeBy { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}
