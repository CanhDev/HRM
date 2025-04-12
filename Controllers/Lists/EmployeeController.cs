using ERP.Base_sys;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _EmployeeService;

        public EmployeeController(EmployeeService EmployeeService)
        {
            _EmployeeService = EmployeeService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Employee>>>> GetAll()
        {
            return await _EmployeeService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Employee>>> GetById(int id)
        {
            try
            {
                var res = await _EmployeeService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<Employee>
                    {
                        Success = false,
                        Message = $"Không tìm thấy Employee với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Employee>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<Employee>>>> GetData([FromBody] SearchRequest<Employee> request)
        {
            var response = await _EmployeeService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Employee>>> Create([FromBody] Employee Employee)
        {
            var response = await _EmployeeService.AddAsync(Employee, x => x.Id == Employee.Id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Employee>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/Employee
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Employee>>> Update(int id, [FromBody] Employee Employee)
        {
            var response = await _EmployeeService.UpdateAsync(Employee, x => x.Id == id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Employee>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Employee>>> Delete(int id)
        {
            var response = await _EmployeeService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Employee>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }
    }
}
