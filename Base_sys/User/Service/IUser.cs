using ERP.Base_sys.Services;
using ERP.Base_sys.User.models;
using ERP.Entities._1_Configs;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace ERP.Base_sys.User.Service
{
    public interface IUser : IBaseService<ApplicationUser>
    {

        Task<ApiRespone_basic> GetById_Custom(string EmployeeCode);
        Task<ApiRespone_basic> AddAsync_Custom(UserDTO req);
        Task<ApiRespone_basic> EditAsync_Custom(string id, UserDTO req);
        Task<ApiRespone_basic> DeleteAsync_Custom(string id);
        // Password Management
        Task<ApiRespone_basic> ChangePasswordAsync(ChangePasswordModel model);
        Task<ApiRespone_basic> RequestPasswordResetAsync(string EmployeeCode);
        Task<ApiRespone_basic> ResetPasswordAsync(ResetPasswordModel model);
        // Role Management
        Task<ApiRespone_basic> AssignRoleToUserAsync(string userId, string roleName);
        Task<ApiRespone_basic> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<List<string>> GetAllRolesAsync();
    }
}
