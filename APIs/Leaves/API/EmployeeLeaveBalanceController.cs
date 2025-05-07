using ERP.APIs.Leaves.entity;
using ERP.APIs.Leaves.Services;
using ERP.Base_sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.APIs.Leaves.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeLeaveBalanceController : ControllerBase
    {
        private readonly EmployeeLeaveBalanceService _EmployeeLeaveBalanceService;

        public EmployeeLeaveBalanceController(EmployeeLeaveBalanceService EmployeeLeaveBalanceService)
        {
            _EmployeeLeaveBalanceService = EmployeeLeaveBalanceService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EmployeeLeaveBalance>>>> GetAll()
        {
            return await _EmployeeLeaveBalanceService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeLeaveBalance>>> GetById(int id)
        {
            try
            {
                var res = await _EmployeeLeaveBalanceService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<EmployeeLeaveBalance>
                    {
                        Success = false,
                        Message = $"Không tìm thấy EmployeeLeaveBalance với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<EmployeeLeaveBalance>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeLeaveBalance>>>> GetData([FromBody] SearchRequest<EmployeeLeaveBalance> request)
        {
            var response = await _EmployeeLeaveBalanceService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmployeeLeaveBalance>>> Create([FromBody] EmployeeLeaveBalance EmployeeLeaveBalance)
        {
            var response = await _EmployeeLeaveBalanceService.AddAsync(EmployeeLeaveBalance);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmployeeLeaveBalance>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/EmployeeLeaveBalance
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeLeaveBalance>>> Update(int id, [FromBody] EmployeeLeaveBalance EmployeeLeaveBalance)
        {
            var response = await _EmployeeLeaveBalanceService.UpdateAsync(EmployeeLeaveBalance);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmployeeLeaveBalance>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/EmployeeLeaveBalance/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeLeaveBalance>>> Delete(int id)
        {
            var response = await _EmployeeLeaveBalanceService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmployeeLeaveBalance>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        [HttpDelete("DeleteRange{ids}")]
        public async Task<IActionResult> DeleteRange(List<int> ids)
        {
            var response = await _EmployeeLeaveBalanceService.DeleteRangeAsync(ids);
            if (response == false)
            {
                return Ok(new ApiRespone_basic
                {
                    Data = null,
                    Success = false
                });
            }
            return Ok(new ApiRespone_basic
            {
                Data = null,
                Success = true
            });
        }
        [HttpDelete("DeleteAll{ids}")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            var response = await _EmployeeLeaveBalanceService.DeleteAllAsync();
            if (response == false)
            {
                return Ok(new ApiRespone_basic
                {
                    Data = null,
                    Success = false
                });
            }
            return Ok(new ApiRespone_basic
            {
                Data = null,
                Success = true
            });
        }
    }
}
