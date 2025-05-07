using ERP.APIs.Leaves.DTOs;
using ERP.APIs.Leaves.DTOs_Res;
using ERP.APIs.Leaves.entity;
using ERP.APIs.Leaves.Services.interfaces;
using ERP.Base_sys;
using Microsoft.AspNetCore.Mvc;

namespace ERP.APIs.Leaves.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly ILeave _leaveService;

        public LeaveController(ILeave leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _leaveService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("GetPagedList")]
        public async Task<IActionResult> GetPagedList([FromBody] SearchRequest<LeaveRequest> request)
        {
            var result = await _leaveService.GetPagedListAsync(request);
            return Ok(result);
        }

        [HttpGet("GetNew/{employeeId}")]
        public async Task<IActionResult> GetNew(int employeeId)
        {
            var result = await _leaveService.GetNew(employeeId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _leaveService.GetById_custom(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Dataset_Leave request)
        {
            var result = await _leaveService.AddAsync_custom(request);
            return Ok(result);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Dataset_Leave request)
        {
            var result = await _leaveService.EditAsync_custom(id, request);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _leaveService.DeleteAsync(id);

            return Ok(new ApiRespone_basic
            {
                Success = result.Success,
                Data = result.Data
            });
        }

        [HttpDelete("DeleteRange{ids}")]
        public async Task<IActionResult> DeleteRange(List<int> ids)
        {
            var response = await _leaveService.DeleteRangeAsync(ids);
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
            var response = await _leaveService.DeleteAllAsync();
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

        [HttpPost("Approve/{id}")]
        public async Task<IActionResult> Approve(int id, [FromQuery] int approverId)
        {
            var result = await _leaveService.Approve(id, approverId);
            return Ok(result);
        }

        [HttpPost("Reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromQuery] int approverId, [FromQuery] string reason)
        {
            var result = await _leaveService.Reject(id, approverId, reason);
            return Ok(result);
        }
    }
}
