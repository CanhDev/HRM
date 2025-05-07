using ERP.Base_sys;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{

    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly EducationService _EducationService;

        public EducationController(EducationService EducationService)
        {
            _EducationService = EducationService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Education>>>> GetAll()
        {
            return await _EducationService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Education>>> GetById(int id)
        {
            try
            {
                var res = await _EducationService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<Education>
                    {
                        Success = false,
                        Message = $"Không tìm thấy Education với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Education>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<Education>>>> GetData([FromBody] SearchRequest<Education> request)
        {
            var response = await _EducationService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Education>>> Create([FromBody] Education Education)
        {
            var response = await _EducationService.AddAsync(Education);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Education>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/Education
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Education>>> Update(int id, [FromBody] Education Education)
        {
            var response = await _EducationService.UpdateAsync(Education);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Education>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/Education/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Education>>> Delete(int id)
        {
            var response = await _EducationService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Education>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }
    }
}
