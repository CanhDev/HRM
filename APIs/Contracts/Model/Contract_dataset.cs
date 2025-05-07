using ERP.DTO;
using ERP.Entities;
using ERP.Entities._0_Systems;
using System.ComponentModel.DataAnnotations;

namespace ERP.APIs.Contracts.Model
{
    public class Contract_dataset
    {
        public ContractDTO contractDTO { get; set; }
        public List<ContractAddendumDTO>? contractAddendumDTOs { get; set; }
        //public IFormFile? FileItem { get; set; }
    }

    public class ContractDTO
    {
        public int employeeId { get; set; }
        public int? departmentId { get; set; }

        public int? positionId { get; set; }

        public string? contractCode { get; set; }
        public int? contractTypeId { get; set; }
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
        public int? status { get; set; }
    }

    public class ContractAddendumDTO : IActionDto
    {
        public int contractId { get; set; }

        public string? title { get; set; }
        public DateTime effectiveDate { get; set; }
        public string? addendumContent { get; set; }
        public string? changeField { get; set; }
        public decimal? salary { get; set; }
        public int? departmentId { get; set; }
        public int? positionId { get; set; }
        public DateTime? endDate { get; set; }
        public int id { get; set; }
        public string? actionType { get; set; }
    }

    public class ContractHistoryDTO
    {
        public int contractId { get; set; }

        public DateTime changeDate { get; set; }
        public int changeBy { get; set; }

        public string? oldValue { get; set; }
        public string? newValue { get; set; }
    }
}
