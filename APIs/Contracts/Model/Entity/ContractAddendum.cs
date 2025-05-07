using ERP.Entities;
using ERP.Entities._0_Systems;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.APIs.Contracts.Model.Contract
{
    public class ContractAddendum : BaseEntity
    {
        public int contractId { get; set; }

        public string? title { get; set; }
        public DateTime effectiveDate { get; set; }
        public string? addendumContent { get; set; }
        public string? changeField { get; set; }
        public decimal? salary { get; set; }
        public int? departmentID { set; get; }
        public int? positionID { get; set; }
        public DateTime? endDate { get; set; }
    }
}
