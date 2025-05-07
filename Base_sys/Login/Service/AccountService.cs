using AutoMapper;
using ERP.Base_sys.Helpers;
using ERP.Base_sys.jwtService;
using ERP.Base_sys.Login.model;
using ERP.Data_res.Lists;
using ERP.DTO.Lists;
using ERP.Entities;
using ERP.Entities._1_Configs;
using ERP.Entities.Lists.Employee;
using ERP.Services.Lists;
using ERP.Services.Lists.interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP.Base_sys.Login.Service
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IAccountRepo _accountRepo;
        private readonly IEmployeeService _employeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHelper _jwtHelper;

        public AccountService(IAccountRepo accountRepo,
            
            ApplicationDbContext context,
            IMapper mapper,
            IEmployeeService employeeService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            JwtHelper jwtHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _accountRepo = accountRepo;
            _employeeService = employeeService;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtHelper = jwtHelper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiRespone_basic> SignUpAsync(SignUpModel model)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext?.Request;
                var defaultImageUrl = $"{request.Scheme}://{request.Host}/resource/images/no_avatar.png";

                // check exist email
                var existingUser = await _userManager.FindByEmailAsync(model.email);
                if (existingUser != null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Email đã tồn tại. Vui lòng sử dụng email khác."
                    };
                }
                var userAccount = new ApplicationUser
                {

                    Email = model.email,
                    UserName = model.email,
                    avatarUrl = defaultImageUrl,
                    EmployeeCode = await _employeeService.GenerateEmployeeCodeWithYearAsync()
                };
                var result = await _accountRepo.CreateAccount(userAccount, model.password);
                if (result)
                {
                    if (!await _roleManager.RoleExistsAsync(AppRole.Employee))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRole.Employee));
                    }
                    if (!await _roleManager.RoleExistsAsync(AppRole.HRManager))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRole.HRManager));
                    }
                    if (!await _roleManager.RoleExistsAsync(AppRole.Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRole.Admin));
                    }
                    if (userAccount.Email == "Admin@gmail.com")
                    {
                        await _userManager.AddToRoleAsync(userAccount, AppRole.Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(userAccount, AppRole.Employee);
                    }

                    var employeeE = new EmployeeDTO
                    {
                        employeeCode = userAccount.EmployeeCode,
                        email = model.email,
                        imageUrl = defaultImageUrl
                    };
                    var employee_dataset = new Employee_dataset { employeeDTO = employeeE };
                    await _employeeService.AddAsync_full(employee_dataset);
                }
                return new ApiRespone_basic
                {
                    Success = result,
                    Message = result ? "SignUp Successful" : "SignUp failed",
                    Data = userAccount.EmployeeCode
                };
            }
            catch (Exception ex)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message,
                };
            }

        }

        public async Task<ApiRespone_basic> LoginAsync(LoginModel model)
        {
            try
            {
                var accountValid = await _accountRepo.CheckAccount(model);
                if (accountValid != null)
                {
                    var result = await _jwtHelper.GenerateToken(accountValid);
                    var EmployeeE = await _context.Employees.FirstOrDefaultAsync(u => u.employeeCode == accountValid.EmployeeCode);
                    return new ApiRespone_basic
                    {
                        Success = true,
                        Message = "Login Successfull",
                        Data = new
                        {
                            token = result,
                            employee = _mapper.Map<Employee_res>(EmployeeE)
                        }
                    };
                }
                else
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Wrong password or username"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ApiRespone_basic> RenewTokenAsync(TokenModel model)
        {
            return await _jwtHelper.RefreshToken(model);
        }
    }
}
