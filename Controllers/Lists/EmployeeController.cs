using ERP.Base_sys;
using ERP.Base_sys.jwtService;
using ERP.DTO.Lists;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists;
using ERP.Services.Lists.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ERP.Controllers.Lists
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeService _EmployeeService;

        public EmployeeController(IEmployeeService EmployeeService)
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
                var res = await _EmployeeService.GetAllById(id);
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
        public async Task<IActionResult> Create([FromForm] EmployeeCreateRequest request)
        {
            try
            {
                if (request.imageFile != null)
                {
                    request.employeeDTO.imageFile = request.imageFile;
                }

                var employeeData = new Employee_dataset
                {
                    employeeDTO = request.employeeDTO
                };

                if (!string.IsNullOrEmpty(request.emergencyContactsJson))
                {
                    employeeData.emergencyContactList = JsonSerializer.Deserialize<List<EmergencyContactDTO>>(
                        request.emergencyContactsJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                if (!string.IsNullOrEmpty(request.educationListJson))
                {
                    employeeData.educationList = JsonSerializer.Deserialize<List<EducationDTO>>(
                        request.educationListJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                if (!string.IsNullOrEmpty(request.workExperienceListJson))
                {
                    employeeData.workExperienceList = JsonSerializer.Deserialize<List<WorkExperienceDTO>>(
                        request.workExperienceListJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                var response = await _EmployeeService.AddAsync_full(employeeData);
                if (!response.Success)
                {
                    return StatusCode(500, new ApiResponse<Employee>
                    {
                        Success = false,
                        Data = null,
                        Message = response.Message
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Employee>
                {
                    Success = false,
                    Data = null,
                    Message = ex.Message
                });
            }
        }

        // PUT: api/Employee
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] EmployeeCreateRequest request)
        {
            try
            {
                if (request.imageFile != null)
                {
                    request.employeeDTO.imageFile = request.imageFile;
                }

                var employeeData = new Employee_dataset
                {
                    employeeDTO = request.employeeDTO
                };

                if (!string.IsNullOrEmpty(request.emergencyContactsJson))
                {
                    employeeData.emergencyContactList = JsonSerializer.Deserialize<List<EmergencyContactDTO>>(
                        request.emergencyContactsJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                if (!string.IsNullOrEmpty(request.educationListJson))
                {
                    employeeData.educationList = JsonSerializer.Deserialize<List<EducationDTO>>(
                        request.educationListJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                if (!string.IsNullOrEmpty(request.workExperienceListJson))
                {
                    employeeData.workExperienceList = JsonSerializer.Deserialize<List<WorkExperienceDTO>>(
                        request.workExperienceListJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }

                var response = await _EmployeeService.EditAsync_full(id, employeeData);
                if (!response.Success)
                {
                    return StatusCode(500, new ApiResponse<Employee>
                    {
                        Success = false,
                        Data = null,
                        Message = response.Message
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Employee>
                {
                    Success = false,
                    Data = null,
                    Message = ex.Message
                });
            }
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
