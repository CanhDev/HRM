using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities.Lists.Contract
{
    public class ContractType : BaseEntity
    {
        [Required]
        public string? contractTypeCode { get; set; }

        [Required, StringLength(100)]
        public string? contractTypeName { get; set; }

        [StringLength(255)]
        public string? description { get; set; }

    }
}
