using System.ComponentModel.DataAnnotations;

namespace ERP.Base_sys.Login.model
{
    public class SignUpModel
    {
        [Required, EmailAddress]
        public string email { get; set; } = null!;
        [Required]
        public string password { get; set; } = null!;
        [Required]
        public string confirmPassword { get; set; } = null!;
    }
}
