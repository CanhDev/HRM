using AutoMapper;
using ERP.Base_sys.User.models;
using ERP.Base_sys.User.Service;
using ERP.Entities._1_Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Base_sys.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly IMapper _mapper;

        public UserController(IUser userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userService.GetAllAsync();
                return Ok(new ApiRespone_basic
                {
                    Success = result.Success,
                    Data = result.Success ? _mapper.Map<List<UserRes>>(result.Data) : null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost("get-paged")]
        public async Task<IActionResult> GetPagedList([FromBody] SearchRequest<ApplicationUser> request)
        {
            var result = await _userService.GetPagedListAsync(request);
            return Ok(new ApiRespone_basic
            {
                Success = result.Success,
                Data = result.Success ? _mapper.Map<List<UserRes>>(result.Data.items) : null
            });
        }

        [HttpGet("GetByEmployeeCode")]
        public async Task<IActionResult> GetById(string EmployeeCode)
        {
            var result = await _userService.GetById_Custom(EmployeeCode);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] UserDTO model)
        {
            var result = await _userService.AddAsync_Custom(model);
            return Ok(new ApiRespone_basic
            {
                Success = result.Success,
                Data = result.Success ? _mapper.Map<UserRes>(result.Data) : null,
                Message = result.Message
            });
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromForm] UserDTO model)
        {
            var result = await _userService.EditAsync_Custom(id, model);
            return Ok(new ApiRespone_basic
            {
                Success = result.Success,
                Data = result.Success ? _mapper.Map<UserRes>(result.Data) : null,
                Message = result.Message
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.DeleteAsync_Custom(id);
            return Ok(new ApiRespone_basic
            {
                Success = result.Success,
            });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var result = await _userService.ChangePasswordAsync(model);
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var result = await _userService.ResetPasswordAsync(model);
            return Ok(result);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromQuery] string userId, [FromQuery] string roleName)
        {
            var result = await _userService.AssignRoleToUserAsync(userId, roleName);
            return Ok(result);
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromQuery] string userId, [FromQuery] string roleName)
        {
            var result = await _userService.RemoveRoleFromUserAsync(userId, roleName);
            return Ok(result);
        }

        
    }
}
