using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ERP.Entities.Lists;

namespace ERP.Entities._0_Systems
{
    public abstract class BaseVoucherEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updateAt { get; set; }
        public string createBy { get; set; }
        public string updateBy { get; set; }
        public short ApprovalStatus { get; set; }
        public string ApprovalUser { get; set; }
        public string CancelReason { get; set; }
        public string RejectReason { get; set; }

        [Key]
        [Required]
        public string voucher_code { get; set; }
        [Required]
        public string voucher_number { get; set; }
        public string voucher_date { get; set; }
        public string detail_note { get; set; }
        public string DepartmentID { get; set; }

    }
}
