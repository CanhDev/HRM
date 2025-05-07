using ERP.Base_sys.Login.model;
using ERP.Entities;
using ERP.Entities._1_Configs;
using ERP.Entities.Lists.Employee;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP.Base_sys.Login.Service
{
    public class AccountRepo : IAccountRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountRepo(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,

            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser?> CheckAccount(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeCode == model.employeeCode);
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.password);
            if (user == null || !passwordValid) return null;
            return user;
        }

        public async Task<bool> CreateAccount(ApplicationUser userAccount, string password)
        {
            var result = await _userManager.CreateAsync(userAccount, password);
            return result.Succeeded;
        }
    }
}
