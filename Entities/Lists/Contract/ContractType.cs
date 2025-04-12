using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Lists.Contract
{
    public class ContractType : BaseEntity
    {
        [Required]
        public string ContractTypeCode { get; set; }

        [Required, StringLength(100)]
        public string ContractTypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

    }
}
