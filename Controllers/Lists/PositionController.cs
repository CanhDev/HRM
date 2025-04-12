using ERP.Base_sys;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly PositionService _positionService;

        public PositionController(PositionService positionService)
        {
            _positionService = positionService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Position>>>> GetAll()
        {
            return await _positionService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Position>>> GetById(int id)
        {
            try
            {
                var res = await _positionService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<Position>
                    {
                        Success = false,
                        Message = $"Không tìm thấy Position với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<Position>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<Position>>>> GetData([FromBody] SearchRequest<Position> request)
        {
            var response = await _positionService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Position>>> Create([FromBody] Position position)
        {
            var response = await _positionService.AddAsync(position, x => x.Id == position.Id);
            if(response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Position>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/Position
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Position>>> Update(int id, [FromBody] Position position)
        {
            var response = await _positionService.UpdateAsync(position, x => x.Id == id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Position>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/Position/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Position>>> Delete(int id)
        {
            var response = await _positionService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<Position>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }
    }
}
