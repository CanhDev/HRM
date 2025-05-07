using ERP.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.APIs.Lookups
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LookUpController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("contract-types")]
        public async Task<IActionResult> GetContractTypes()
        {
            var data = await _context.ContractTypes
                .Where(x => x.status != -1)
                .Select(x => new
                {
                    form_value = x.id,
                    form_name = x.contractTypeName
                })
                .ToListAsync();
            return Ok(data);
        }
        [HttpGet("leave-types")]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var data = await _context.LeaveTypes
                .Where(x => x.status != -1)
                .Select(x => new
                {
                    form_value = x.id,
                    form_name = x.leaveTypeName
                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("employees")]
        public async Task<IActionResult> Employees()
        {
            var data = await _context.Employees
                .Where(x => x.status != -1)
                .Select(x => new
                {
                    form_value = x.id,
                    form_name = x.fullName
                })
                .ToListAsync();
            return Ok(data);
        }
        [HttpGet("departments")]
        public async Task<IActionResult> Departments()
        {
            var data = await _context.Departments
                .Where(x => x.status != -1)
                .Select(x => new
                {
                    form_value = x.id,
                    form_name = x.departmentName
                })
                .ToListAsync();
            return Ok(data);
        }
        [HttpGet("positions")]
        public async Task<IActionResult> Positions()
        {
            var data = await _context.Positions
                .Where(x => x.status != -1)
                .Select(x => new
                {
                    form_value = x.id,
                    form_name = x.positionName
                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("sys_listoptions")]
        public async Task<IActionResult> Sys_listoptions(string form, string form_type)
        {
            var data = await _context.ListOptions
                .Where(x => x.status != false & x.form == form & x.form_type == form_type)
                .Select(x => new
                {
                    form_value = x.form_value,
                    form_name = x.form_name,
                    display_value = x.form_value + " " + x.form_name,
                })
                .ToListAsync();
            return Ok(data);
        }
        [HttpGet("sys_dmtt")]
        public async Task<IActionResult> Sys_dmtt(string ma_ct)
        {
            var data = await _context.sys_Dmtts
                .Where(x => x.status != 0 & x.ma_ct == ma_ct)
                .Select(x => new
                {
                    form_value = x.status_code,
                    form_name = x.status_name,
                    display_value = x.status_code + " " + x.status_name,
                })
                .ToListAsync();
            return Ok(data);
        }
    }
}
