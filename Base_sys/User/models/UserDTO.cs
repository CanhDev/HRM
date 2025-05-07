using System.ComponentModel.DataAnnotations;

namespace ERP.Base_sys.User.models
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string? EmployeeCode { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]  
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        public int? DepartmentID { get; set; }
        public int? PositionID { get; set; }
        public string? RoleName { get; set; }
        public string? avatarUrl { get; set; }
        public IFormFile? imageFile { get; set; }
        public int status { get; set; }
    }
}
