using System.ComponentModel.DataAnnotations;

namespace ERP.Base_sys.User.models
{
    public class UserRes
    {
        public string? Id { get; set; }
        public string? EmployeeCode { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public string? RoleName { get; set; }
        public string? FullName { get; set; }
        public int? DepartmentID { get; set; }
        public int? PositionID { get; set; }
        public string? avatarUrl { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updateAt { get; set; }
        public string? createBy { get; set; }
        public string? updateBy { get; set; }
        public int status { get; set; }
    }
}
