using ERP.APIs.Leaves.entity;
using ERP.APIs.Leaves.Services;
using ERP.Base_sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.APIs.Leaves.API
{

    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly LeaveTypeService _LeaveTypeService;

        public LeaveTypeController(LeaveTypeService LeaveTypeService)
        {
            _LeaveTypeService = LeaveTypeService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<LeaveType>>>> GetAll()
        {
            return await _LeaveTypeService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LeaveType>>> GetById(int id)
        {
            try
            {
                var res = await _LeaveTypeService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<LeaveType>
                    {
                        Success = false,
                        Message = $"Không tìm thấy LeaveType với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<LeaveType>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<LeaveType>>>> GetData([FromBody] SearchRequest<LeaveType> request)
        {
            var response = await _LeaveTypeService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<LeaveType>>> Create([FromBody] LeaveType LeaveType)
        {
            var response = await _LeaveTypeService.AddAsync(LeaveType);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<LeaveType>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/LeaveType
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<LeaveType>>> Update(int id, [FromBody] LeaveType LeaveType)
        {
            var response = await _LeaveTypeService.UpdateAsync(LeaveType);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<LeaveType>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/LeaveType/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<LeaveType>>> Delete(int id)
        {
            var response = await _LeaveTypeService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<LeaveType>
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
            var response = await _LeaveTypeService.DeleteRangeAsync(ids);
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
            var response = await _LeaveTypeService.DeleteAllAsync();
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
