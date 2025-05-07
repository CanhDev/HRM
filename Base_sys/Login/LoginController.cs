using ERP.Base_sys.jwtService;
using ERP.Base_sys.Login.model;
using ERP.Base_sys.Login.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Base_sys.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public LoginController(IAccountService accountService) {
            _accountService = accountService;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            try
            {
                return Ok(await _accountService.SignUpAsync(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                return Ok(await _accountService.LoginAsync(model));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel OldToken)
        {
            try
            {
                return Ok(await _accountService.RenewTokenAsync(OldToken));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
