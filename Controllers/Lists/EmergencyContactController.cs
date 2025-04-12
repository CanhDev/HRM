using ERP.Base_sys;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyContactController : ControllerBase
    {
        private readonly EmergencyContactService _EmergencyContactService;

        public EmergencyContactController(EmergencyContactService EmergencyContactService)
        {
            _EmergencyContactService = EmergencyContactService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EmergencyContact>>>> GetAll()
        {
            return await _EmergencyContactService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EmergencyContact>>> GetById(int id)
        {
            try
            {
                var res = await _EmergencyContactService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<EmergencyContact>
                    {
                        Success = false,
                        Message = $"Không tìm thấy EmergencyContact với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<EmergencyContact>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<EmergencyContact>>>> GetData([FromBody] SearchRequest<EmergencyContact> request)
        {
            var response = await _EmergencyContactService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmergencyContact>>> Create([FromBody] EmergencyContact EmergencyContact)
        {
            var response = await _EmergencyContactService.AddAsync(EmergencyContact, x => x.Id == EmergencyContact.Id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmergencyContact>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/EmergencyContact
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<EmergencyContact>>> Update(int id, [FromBody] EmergencyContact EmergencyContact)
        {
            var response = await _EmergencyContactService.UpdateAsync(EmergencyContact, x => x.Id == id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmergencyContact>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/EmergencyContact/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<EmergencyContact>>> Delete(int id)
        {
            var response = await _EmergencyContactService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmergencyContact>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }
    }
}
