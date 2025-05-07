using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updateAt { get; set; }
        public string? createBy { get; set; }
        public string? updateBy { get; set; }
        public int status { get; set; } = 1;
    }
}
