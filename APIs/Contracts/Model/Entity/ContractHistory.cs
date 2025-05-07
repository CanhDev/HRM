using ERP.Entities;
using ERP.Entities._0_Systems;
using ERP.Entities.Lists.Employee;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.APIs.Contracts.Model.Contract
{
    public class ContractHistory : BaseEntity
    {
        public int contractId { get; set; }
        public string? oldValue { get; set; }
        public string? newValue { get; set; }
        public string? desciption { get; set; }
    }
}
