using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities;
using System.Linq.Expressions;
using ERP.Entities.Lists.Employee;
using ERP.DTO.Lists;
using ERP.Services.Lists.interfaces;
using ERP.Data_res.Lists;
using ERP.Entities.Vouchers.Employee;
using AutoMapper;
using ERP.Base_sys.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.Lists
{

    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly EmergencyContactService _emergencyContactService;
        private readonly EducationService _educationService;
        private readonly WorkExperienceService _workExperienceService;
        private readonly IEmployeeDocumentService _employeeDocumentService;
        private readonly FileHelper _fileHelper;
        private readonly IMapper _mapper;
        private readonly ImageHelper _imageHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public EmployeeService(IBaseRepository<Employee> repository,
            ApplicationDbContext context,
            EmergencyContactService emergencyContactService,
            EducationService educationService,
            WorkExperienceService workExperienceService,
            IEmployeeDocumentService employeeDocumentService,
            IHttpContextAccessor httpContextAccessor,
             FileHelper fileHelper,
             ImageHelper imageHelper,
            IMapper mapper,
            ILogger<EmployeeService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
            _emergencyContactService = emergencyContactService;
            _educationService = educationService;
            _workExperienceService = workExperienceService;
            _employeeDocumentService = employeeDocumentService;
            _fileHelper = fileHelper;
            _mapper = mapper;
            _imageHelper = imageHelper;
            _httpContextAccessor = httpContextAccessor;
        }


        public override async Task<ApiResponse<List<Employee>>> GetAllAsync(Expression<Func<Employee, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<Employee?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<Employee>>> GetPagedListAsync(
            SearchRequest<Employee> searchRequest, params Expression<Func<Employee, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<Employee>> AddAsync(Employee entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<Employee>> UpdateAsync(Employee entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<Employee>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }

        public override async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
        {
            return await base.DeleteRangeAsync(ids);
        }

        public async Task<ApiRespone_basic> GetAllById(int id)
        {
            var EmployeeE = await _repository.GetByIdAsync(id);
            var EmergencyContactE = await _emergencyContactService.GetAllAsync(p => p.employeeId == id & p.status != -1);
            var EducationE = await _educationService.GetAllAsync(p => p.employeeId == id & p.status != -1);
            var WorkExperienceE = await _workExperienceService.GetAllAsync(p => p.employeeId == id & p.status != -1);
            if(EmployeeE == null ||  !EmergencyContactE.Success || !EducationE.Success  || !WorkExperienceE.Success)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Entity does not exists"
                };
            }
            var dataRes = new Employee_DataRes {
                employeeRes =  _mapper.Map<Employee_res>(EmployeeE),
                emergencyContactList = _mapper.Map<List<EmergencyContact_res>>(EmergencyContactE.Data),
                educationList = _mapper.Map<List<Education_res>>(EducationE.Data),
                workExperienceList = _mapper.Map<List<WorkExperience_res>>(WorkExperienceE.Data)
            };
            return new ApiRespone_basic
            {
                Success = true,
                Data = dataRes
            };
        }

        public async Task<ApiRespone_basic> AddAsync_full(Employee_dataset req)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var employee = _mapper.Map<Employee>(req.employeeDTO);
                if(employee.employeeCode == null)
                {
                    employee.employeeCode = await GenerateEmployeeCodeWithYearAsync();
                }

                if (req.employeeDTO.imageFile != null)
                {
                    var imageUrl = await _imageHelper.SaveImageAsync(req.employeeDTO.imageFile, "Employee");
                    if (imageUrl == null)
                    {
                        return new ApiRespone_basic
                        {
                            Success = false,
                            Message = "Không thể lưu ảnh"
                        };
                    }
                    employee.imageUrl = imageUrl;
                }
                else
                {
                    var request = _httpContextAccessor.HttpContext?.Request;
                    employee.imageUrl = $"{request?.Scheme}://{request?.Host}/resource/images/no_avatar.png";
                }

                var savedEmployee = await _repository.AddAsync(employee);
                if (savedEmployee == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Không thể lưu nhân viên"
                    };
                }

                int employeeId = savedEmployee.id;

                // EmergencyContacts
                var emergencyContacts = req.emergencyContactList != null && req.emergencyContactList.Any()
                    ? _mapper.Map<List<EmergencyContact>>(req.emergencyContactList)
                    : new List<EmergencyContact> { new EmergencyContact() };
                emergencyContacts.ForEach(ec => ec.employeeId = employeeId);
                await _emergencyContactService.AddRangeAsync(emergencyContacts);

                // Educations
                var educations = req.educationList != null && req.educationList.Any()
                    ? _mapper.Map<List<Education>>(req.educationList)
                    : new List<Education> { new Education() };
                educations.ForEach(e => e.employeeId = employeeId);
                await _educationService.AddRangeAsync(educations);

                // WorkExperiences
                var workExperiences = req.workExperienceList != null && req.workExperienceList.Any()
                    ? _mapper.Map<List<WorkExperience>>(req.workExperienceList)
                    : new List<WorkExperience> { new WorkExperience() };
                workExperiences.ForEach(w => w.employeeId = employeeId);
                await _workExperienceService.AddRangeAsync(workExperiences);

                

                // ✅ Commit transaction nếu tất cả đều thành công
                await transaction.CommitAsync();

                return new ApiRespone_basic
                {
                    Success = true,
                    Message = "Thêm mới thành công"
                };
            }
            catch (Exception ex)
            {
                // ❌ Rollback nếu có lỗi
                await transaction.RollbackAsync();

                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Lỗi khi thêm nhân viên: " + ex.Message
                };
            }
        }


        public async Task<ApiRespone_basic> EditAsync_full(int id, Employee_dataset req)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var employee = await _repository.GetByIdAsync(id);
                if (employee == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Entity does not exist"
                    };
                }

                
                _mapper.Map(req.employeeDTO, employee);

                if (req.employeeDTO.imageFile != null)
                {
                    var imageUrl = await _imageHelper.SaveImageAsync(req.employeeDTO.imageFile, "Employee");
                    if (imageUrl == null)
                    {
                        return new ApiRespone_basic
                        {
                            Success = false,
                            Message = "Không thể lưu ảnh"
                        };
                    }
                    employee.imageUrl = imageUrl;
                }

                var savedEmployee = await _repository.UpdateAsync(employee);
                if (savedEmployee == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Không thể lưu nhân viên"
                    };
                }

                int employeeId = savedEmployee.id;

                if (req.emergencyContactList != null)
                {
                    foreach (var x in req.emergencyContactList)
                        x.employeeId = employeeId;

                    await _emergencyContactService.ProcessActionTypeListAsync(req.emergencyContactList.ToList());
                }

                if (req.educationList != null)
                {
                    foreach (var x in req.educationList)
                        x.employeeId = employeeId;

                    await _educationService.ProcessActionTypeListAsync(req.educationList.ToList());
                }

                if (req.workExperienceList != null)
                {
                    foreach (var x in req.workExperienceList)
                        x.employeeId = employeeId;

                    await _workExperienceService.ProcessActionTypeListAsync(req.workExperienceList.ToList());
                }


                await transaction.CommitAsync();

                return new ApiRespone_basic
                {
                    Success = true,
                    Message = "Sửa thành công"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Lỗi khi sửa nhân viên: " + ex.Message
                };
            }
        }


        public Task<ApiRespone_basic> DeleteAsync_full(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateEmployeeCodeWithYearAsync()
        {
            string yearSuffix = DateTime.Now.Year.ToString().Substring(2); // "24"
            string prefix = $"NV{yearSuffix}-"; // VD: "NV24-"

            // Tìm mã mới nhất bắt đầu bằng "NV24-"
            var latestCode = await _context.Employees
                .Where(e => e.employeeCode.StartsWith(prefix))
                .OrderByDescending(e => e.employeeCode)
                .Select(e => e.employeeCode)
                .FirstOrDefaultAsync();

            int newNumber = 1;

            if (!string.IsNullOrEmpty(latestCode))
            {
                var parts = latestCode.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out var parsed))
                {
                    newNumber = parsed + 1;
                }
            }

            string newCode = prefix + newNumber.ToString("D4");

            // Đảm bảo không trùng
            while (await _context.Employees.AnyAsync(e => e.employeeCode == newCode))
            {
                newNumber++;
                newCode = prefix + newNumber.ToString("D4");
            }

            return newCode;
        }

    }





}
