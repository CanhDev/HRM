using ERP.Base_sys;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{

    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentService _DepartmentService;

        public DepartmentController(DepartmentService DepartmentService)
        {
            _DepartmentService = DepartmentService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Department>>>> GetAll()
        {
            return await _DepartmentService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Department>>> GetById(int id)
        {
            try
            {
                var res = await _DepartmentService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<Department>
                    {
                        Success = false,
                        Message = $"Không tìm thấy Department với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Department>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<Department>>>> GetData([FromBody] SearchRequest<Department> request)
        {
            var response = await _DepartmentService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Department>>> Create([FromBody] Department Department)
        {
            var response = await _DepartmentService.AddAsync(Department);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Department>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/Department
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Department>>> Update(int id, [FromBody] Department Department)
        {
            var response = await _DepartmentService.UpdateAsync(Department);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Department>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/Department/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Department>>> Delete(int id)
        {
            var response = await _DepartmentService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Department>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }
    }
}
