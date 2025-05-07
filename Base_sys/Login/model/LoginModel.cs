using System.ComponentModel.DataAnnotations;

namespace ERP.Base_sys.Login.model
{
    public class LoginModel
    {
        [Required]
        public string employeeCode { get; set; } = null!;
        [Required]
        public string password { get; set; } = null!;
    }
}
