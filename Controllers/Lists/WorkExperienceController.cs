using ERP.Base_sys;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{

    [Route("api/[controller]")]
    [ApiController]
    public class WorkExperienceController : ControllerBase
    {
        private readonly WorkExperienceService _WorkExperienceService;

        public WorkExperienceController(WorkExperienceService WorkExperienceService)
        {
            _WorkExperienceService = WorkExperienceService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<WorkExperience>>>> GetAll()
        {
            return await _WorkExperienceService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkExperience>>> GetById(int id)
        {
            try
            {
                var res = await _WorkExperienceService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<WorkExperience>
                    {
                        Success = false,
                        Message = $"Không tìm thấy WorkExperience với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<WorkExperience>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<WorkExperience>>>> GetData([FromBody] SearchRequest<WorkExperience> request)
        {
            var response = await _WorkExperienceService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkExperience>>> Create([FromBody] WorkExperience WorkExperience)
        {
            var response = await _WorkExperienceService.AddAsync(WorkExperience, x => x.Id == WorkExperience.Id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<WorkExperience>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/WorkExperience
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WorkExperience>>> Update(int id, [FromBody] WorkExperience WorkExperience)
        {
            var response = await _WorkExperienceService.UpdateAsync(WorkExperience, x => x.Id == id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<WorkExperience>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/WorkExperience/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<WorkExperience>>> Delete(int id)
        {
            var response = await _WorkExperienceService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<WorkExperience>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }
    }
}
