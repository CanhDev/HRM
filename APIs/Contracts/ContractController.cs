using ERP.APIs.Contracts.Interfaces;
using ERP.APIs.Contracts.Model;
using ERP.APIs.Contracts.Model.Contract;
using ERP.APIs.Contracts.Services;
using ERP.APIs.Leaves.Services;
using ERP.Base_sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.APIs.Contracts
{

    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContract _ContractService;

        public ContractController(IContract ContractService)
        {
            _ContractService = ContractService;
        }
        [HttpGet("GetNew")]
        public async Task<IActionResult> GetNew()
        {
            var result = await _ContractService.GetNew();
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EmploymentContract>>>> GetAll()
        {
            return await _ContractService.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var res = await _ContractService.GetById_custom(id);
                if (res.Data == null)
                {
                    return Ok(new ApiResponse<EmploymentContract>
                    {
                        Success = false,
                        Message = $"Không tìm thấy EmploymentContract với ID = {id}"
                    });
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<EmploymentContract>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("getData")]
        public async Task<ActionResult<ApiResponse<PagedResult<EmploymentContract>>>> GetData([FromBody] SearchRequest<EmploymentContract> request)
        {
            var response = await _ContractService.GetPagedListAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Contract_dataset data)
        {
            var response = await _ContractService.AddAsync_custom(data);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmploymentContract>
                {
                    Success = false,
                    Data = null,
                    Message = response.Message
                });
            }
            return Ok(response);
        }

        // PUT: api/EmploymentContract
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Contract_dataset data)
        {
            var response = await _ContractService.EditAsync_custom(id, data);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmploymentContract>
                {
                    Success = false,
                    Data = null,
                    Message = response.Message
                });
            }
            return Ok(response);
        }

        [HttpPost("Reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromBody] RejectModel data)
        {
            var result = await _ContractService.Reject(data);
            return Ok(result);
        }

        // DELETE: api/EmploymentContract/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<EmploymentContract>>> Delete(int id)
        {
            var response = await _ContractService.DeleteAsync(id);
            if (response.Success == false)
            {
                return StatusCode(500, new ApiResponse<EmploymentContract>
                {
                    Success = false,
                    Data = null,
                    Message = response.Message
                });
            }
            return Ok(response);
        }

        [HttpDelete("DeleteRange{ids}")]
        public async Task<IActionResult> DeleteRange(List<int> ids)
        {
            var response = await _ContractService.DeleteRangeAsync(ids);
            if (response == false)
            {
                return Ok(new ApiRespone_basic
                {
                    Data = null,
                    Success = false,
                    Message = "Lỗi khi xóa(ví trí báo lỗi: controller)"
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
            var response = await _ContractService.DeleteAllAsync();
            if (response == false)
            {
                return Ok(new ApiRespone_basic
                {
                    Data = null,
                    Success = false,
                    Message = "Lỗi khi xóa(ví trí báo lỗi: controller)"
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
