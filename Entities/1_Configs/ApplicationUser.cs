using Microsoft.AspNetCore.Identity;

namespace ERP.Entities._1_Configs
{
    public class ApplicationUser : IdentityUser
    {

        public string? EmployeeCode { get; set; }
        public int? DepartmentID { get; set; }
        public int? PositionID { get; set; }
        public string? RoleName { get; set; }
        public string? FullName { get; set; }
        public string? avatarUrl { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updateAt { get; set; }
        public string? createBy { get; set; }
        public string? updateBy { get; set; }
        public int? status { get; set; } = -1;
    }
}
