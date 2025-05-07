using AutoMapper;
using ERP.Base_sys.Helpers;
using ERP.Base_sys.jwtService;
using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys.User.models;
using ERP.DTO.Lists;
using ERP.Entities;
using ERP.Entities._1_Configs;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERP.Base_sys.User.Service
{

    public class UserService : BaseService<ApplicationUser>, IUser
    {
        private readonly IEntityAuditHelper _auditHelper;
        private readonly ILogger<UserService> _logger;
        private readonly ImageHelper _imageHelper;
        private readonly IEmployeeService _employeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHelper _jwtHelper;
        public UserService(IBaseRepository<ApplicationUser> repository,
            IEmployeeService employeeService,
            IEntityAuditHelper entityAuditHelper,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            JwtHelper jwtHelper,
            IHttpContextAccessor httpContextAccessor,
         ApplicationDbContext context,
         ImageHelper imageHelper,
          IMapper mapper,
        ILogger<UserService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
            _employeeService = employeeService;
            _imageHelper = imageHelper;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtHelper = jwtHelper;
            _auditHelper = entityAuditHelper;
            _httpContextAccessor = httpContextAccessor;
        }
        public override async Task<ApiResponse<List<ApplicationUser>>> GetAllAsync(Expression<Func<ApplicationUser, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }
        public async Task<ApiRespone_basic> GetById_Custom(string EmployeeCode)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeCode == EmployeeCode);
                if (user == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }
                return new ApiRespone_basic
                {
                    Success = true,
                    Data = _mapper.Map<UserRes>(user)
                };
            }
            catch (Exception ex)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public Task<List<string>> GetAllRolesAsync()
        {
            throw new NotImplementedException();
        }

        
        public override async Task<ApiResponse<PagedResult<ApplicationUser>>> GetPagedListAsync(
            SearchRequest<ApplicationUser> searchRequest, params Expression<Func<ApplicationUser, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }

        public async Task<ApiRespone_basic> AddAsync_Custom(UserDTO req)
        {
            try
            {
                // Kiểm tra email đã tồn tại
                var existingUser = await _userManager.FindByEmailAsync(req.Email);
                if (existingUser != null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Email đã tồn tại. Vui lòng sử dụng email khác."
                    };
                }

                // Ánh xạ từ DTO sang thực thể
                var userEntity = _mapper.Map<ApplicationUser>(req);
                userEntity.EmployeeCode = await _employeeService.GenerateEmployeeCodeWithYearAsync();
                _auditHelper.SetCreatedAuditInfo(userEntity);

                // Xử lý ảnh đại diện
                if (req.imageFile != null)
                {
                    var uploadResult = await _imageHelper.SaveImageAsync( req.imageFile,"UserAccount");
                    if (uploadResult == null)
                    {
                        return new ApiRespone_basic
                        {
                            Success = false,
                            Message = "Không thể lưu ảnh"
                        };
                    }

                    userEntity.avatarUrl = uploadResult;
                }
                else
                {
                    var request = _httpContextAccessor.HttpContext?.Request;
                    userEntity.avatarUrl = $"{request?.Scheme}://{request?.Host}/resource/images/no_avatar.png";
                }

                // Tạo tài khoản người dùng
                var createResult = await _userManager.CreateAsync(userEntity, req.Password);
                if (!createResult.Succeeded)
                {
                    var errorMsg = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = $"Tạo tài khoản thất bại: {errorMsg}"
                    };
                }

                // Gán vai trò cho người dùng
                await _userManager.AddToRoleAsync(userEntity, req.RoleName);

                // Thêm thông tin nhân viên
                var employeeDto = new EmployeeDTO
                {
                    employeeCode = userEntity.EmployeeCode,
                    email = userEntity.Email,
                    imageUrl = userEntity.avatarUrl,
                    positionId = req.PositionID,
                    departmentId = req.DepartmentID,
                    fullName = req.FullName,
                    status = req.status
                };

                var employeeData = new Employee_dataset { employeeDTO = employeeDto };
                await _employeeService.AddAsync_full(employeeData);

                // Trả về kết quả thành công
                return new ApiRespone_basic
                {
                    Success = true,
                    Data = _mapper.Map<ApplicationUser>(userEntity)
                };
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có ngoại lệ
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = $"Đã xảy ra lỗi: {ex.Message}"
                };
            }
        }
        public async Task<ApiRespone_basic> DeleteAsync_Custom(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser == null)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "User not found!"
                };
            }
            _auditHelper.SetUpdatedAuditInfo(existingUser);
            existingUser.status = -1;
            var res = await _userManager.UpdateAsync(existingUser);
            await _context.SaveChangesAsync();
                return new ApiRespone_basic
                {
                    Success = res.Succeeded,
                    Data = res.Succeeded ? id : null,
                    Message = res.Succeeded ? "" : "Lỗi khi xóa"
                };
            
        }

        public async Task<ApiRespone_basic> EditAsync_Custom(string id, UserDTO req)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "User not found!"
                    };
                }

                // Cập nhật các trường từ DTO sang entity
                _mapper.Map(req, existingUser); // Chuyển đổi trực tiếp sang existingUser

                // Cập nhật ảnh nếu có
                if (req.imageFile != null)
                {
                    var uploadResult = await _imageHelper.SaveImageAsync(req.imageFile, "UserAccount");
                    if (uploadResult == null)
                    {
                        return new ApiRespone_basic
                        {
                            Success = false,
                            Message = "Không thể lưu ảnh"
                        };
                    }

                    existingUser.avatarUrl = uploadResult;
                }

                // Gán thông tin audit sau khi cập nhật dữ liệu
                _auditHelper.SetUpdatedAuditInfo(existingUser);

                // Thực hiện cập nhật
                var resultUpdate = await _userManager.UpdateAsync(existingUser);
                if (resultUpdate.Succeeded)
                {
                    return new ApiRespone_basic
                    {
                        Success = true,
                        Data = _mapper.Map<UserRes>(existingUser)
                    };
                }

                // Xử lý lỗi chi tiết từ Identity
                var errors = string.Join("; ", resultUpdate.Errors.Select(e => e.Description));
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi update: " + errors
                };
            }
            catch (Exception ex)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi update: " + ex.Message
                };
            }
        }


        public async Task<ApiRespone_basic> ChangePasswordAsync(ChangePasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "User not found!"
                };
            }
            _auditHelper.SetUpdatedAuditInfo(user);
            var result = await _userManager.ChangePasswordAsync( user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return new ApiRespone_basic
                {
                    Success = true
                };
            }
            return new ApiRespone_basic
            {
                Success = false,
                Message = "Có lỗi khi thay đổi mật khẩu!"
            };
        }
        public Task<ApiRespone_basic> RequestPasswordResetAsync(string EmployeeCode)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiRespone_basic> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "User not found!"
                };
            }
            _auditHelper.SetUpdatedAuditInfo(user);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (model.NewPassword == null) model.NewPassword = "1234";
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            return new ApiRespone_basic
            {
                Success = result.Succeeded,
                Message = result.Succeeded
                    ? "Mật khẩu đã được đặt lại thành công"
                    : "Không thể đặt lại mật khẩu: " + string.Join(", ", result.Errors.Select(e => e.Description)),
                Data = user.Id
            };
        }

        public async Task<ApiRespone_basic> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "User not found!"
                };
            }

            // Cập nhật thông tin audit
            _auditHelper.SetUpdatedAuditInfo(user);

            // Xử lý role
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Failed to create role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description))
                    };
                }
            }

            // Xóa tất cả các vai trò hiện tại
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Failed to remove current roles: " + string.Join(", ", removeResult.Errors.Select(e => e.Description))
                };
            }

            // Gán role mới
            var addResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addResult.Succeeded)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Failed to assign new role: " + string.Join(", ", addResult.Errors.Select(e => e.Description))
                };
            }

            // Gán lại roleName nếu bạn dùng riêng trường này
            user.RoleName = roleName;

            // Cập nhật user nếu cần lưu trường RoleName (tuỳ database)
            await _userManager.UpdateAsync(user);

            return new ApiRespone_basic
            {
                Success = true,
                Message = "Role updated successfully."
            };
        }

        public Task<List<string>> GetUserRolesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiRespone_basic> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "User not found!"
                };
            }
            _auditHelper.SetUpdatedAuditInfo(user);
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
               
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Role does not exist"
                };
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                return new ApiRespone_basic { Success = true }; 
            }

            var result =  await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                user.RoleName = "";
                await _userManager.UpdateAsync(user);
            }
            return new ApiRespone_basic { Success = result.Succeeded ? true : false };
        }
       
    }





}
