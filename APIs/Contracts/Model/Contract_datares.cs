using ERP.Entities;
using System.ComponentModel.DataAnnotations;

namespace ERP.APIs.Contracts.Model
{
    public class Contract_datares
    {
        public ContractRes contractRes { get; set; }
        public List<ContractAddendumRes>? contractAddendumRes { get; set; }
        public List<ContractHistoryRes>? contractHistoryRes { get; set; }
    }
    public class ContractRes : BaseEntity
    {
        public int employeeId { get; set; }
        public int? departmentId { get; set; }

        public int? positionId { get; set; }
        public int contractTypeId { get; set; }

        public string? contractCode { get; set; }
        public decimal salary { get; set; }
        [StringLength(250)]
        public string? notes { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? signedDate { get; set; }
        public string? workingTime { get; set; }
        public string? jobDescription { get; set; }
        public string? benefits { get; set; }
        public string? filePath { get; set; }
        public string? terms { get; set; }
    }
    public class ContractAddendumRes : BaseEntity
    {
        public int contractId { get; set; }
        public string? title { get; set; }
        public string? changeField { get; set; }
        public DateTime effectiveDate { get; set; }
        public string? addendumContent { get; set; }
    }
    public class ContractHistoryRes : BaseEntity
    {
        public int contractId { get; set; }
        public string? oldValue { get; set; }
        public string? newValue { get; set; }
    }
}
