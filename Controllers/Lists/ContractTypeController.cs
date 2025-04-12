using ERP.Base_sys;
using ERP.Entities.Lists.Contract;
using ERP.Services.Lists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContractTypeController : ControllerBase
    {
        private readonly ContractTypeService _ContractTypeService;

        public ContractTypeController(ContractTypeService ContractTypeService)
        {
            _ContractTypeService = ContractTypeService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ContractType>>>> GetAll()
        {
            return await _ContractTypeService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ContractType>>> GetById(int id)
        {
            try
            {
                var res = await _ContractTypeService.GetByIdAsync(id);
                if (res.Data == null)
                {
                    return NotFound(new ApiResponse<ContractType>
                    {
                        Success = false,
                        Message = $"Không tìm thấy ContractType với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ContractType>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<ContractType>>>> GetData([FromBody] SearchRequest<ContractType> request)
        {
            var response = await _ContractTypeService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ContractType>>> Create([FromBody] ContractType ContractType)
        {
            var response = await _ContractTypeService.AddAsync(ContractType, x => x.Id == ContractType.Id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<ContractType>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // PUT: api/ContractType
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ContractType>>> Update(int id, [FromBody] ContractType ContractType)
        {
            var response = await _ContractTypeService.UpdateAsync(ContractType, x => x.Id == id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<ContractType>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }

        // DELETE: api/ContractType/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<ContractType>>> Delete(int id)
        {
            var response = await _ContractTypeService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<ContractType>
                {
                    Success = false,
                    Data = null
                });
            }
            return Ok(response);
        }
    }
}
