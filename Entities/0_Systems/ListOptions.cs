using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Entities._0_Systems
{
    public class ListOptions
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string form { get; set; }
        public string form_type { get; set; }
        public string form_value { get; set; }
        public string form_name { get; set; }
        public bool? status { get; set; }
    }
}
