using ERP.Base_sys;
using ERP.DTO.Lists;
using ERP.Entities.Vouchers.Employee;
using ERP.Services.Lists;
using ERP.Services.Lists.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IEmployeeDocumentService _EmployeeDocumentService;

        public EmployeeDocumentController(IEmployeeDocumentService employeeDocumentService)
        {
            _EmployeeDocumentService = employeeDocumentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EmployeeDocument>>>> GetAll()
        {
            return await _EmployeeDocumentService.GetAllAsync();
        }
        [HttpGet("GetbyEmployee{id}")]
        public async Task<ActionResult<ApiResponse<List<EmployeeDocument>>>> GetAllByEmployee(int id)
        {
            return await _EmployeeDocumentService.GetAllAsync(p => p.employeeId == id);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeDocument>>> GetById(int id)
        {
            try
            {
                var res = await _EmployeeDocumentService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<EmployeeDocument>
                    {
                        Success = false,
                        Message = $"Không tìm thấy EmployeeDocument với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<EmployeeDocument>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<EmployeeDocument>>>> GetData([FromBody] SearchRequest<EmployeeDocument> request)
        {
            var response = await _EmployeeDocumentService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeDoc_dataset data)
        {
            try
            {
                var response = await _EmployeeDocumentService.AddAsync_full(data.documents);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest( new ApiRespone_basic
                {
                    Data = null,
                    Message = ex.Message,

                });
            }
        }

        // PUT: api/EmployeeDocument
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] EmployeeDoc_dataset data)
        {
            var response = await _EmployeeDocumentService.EditAsync_full(data.documents);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmployeeDocument>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/EmployeeDocument/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<EmployeeDocument>>> Delete(int id)
        {
            var response = await _EmployeeDocumentService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmployeeDocument>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }
        [HttpDelete("DeleteRange")]
        public async Task<IActionResult> DeleteRange([FromQuery] List<int> ids)
        {
            var response = await _EmployeeDocumentService.DeleteRangeAsync(ids);
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
        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAllAsync()
        {
            var response = await _EmployeeDocumentService.DeleteAllAsync();
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
