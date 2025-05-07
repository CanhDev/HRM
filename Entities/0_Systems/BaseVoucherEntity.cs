using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ERP.Entities.Lists;

namespace ERP.Entities._0_Systems
{
    public abstract class BaseVoucherEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updateAt { get; set; }
        public string? createBy { get; set; }
        public string? updateBy { get; set; }
        public int status { get; set; } = 1;

        
        public DateTime? voucher_date { get; set; }
        [Required]
        public string voucher_code { get; set; }
        public string? detail_note { get; set; }
        public int approvalStatus { get; set; } // trạng thái duyệt join với bảng sys_dmtt
        public int? departmentID { get; set; }
        public int? approvalUser { get; set; }
        public string? cancelReason { get; set; }
        public string? rejectReason { get; set; }
    }
}
