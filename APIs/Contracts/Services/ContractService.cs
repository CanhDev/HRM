using AutoMapper;
using ERP.Base_sys.Helpers;
using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Base_sys;
using System.Linq.Expressions;
using ERP.APIs.Contracts.Interfaces;
using ERP.Entities;
using ERP.APIs.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using ERP.Base_sys.User.Service;
using Newtonsoft.Json;
using ERP.Entities.Lists.Employee;
using ERP.APIs.Contracts.Model.Contract;
using System.Reflection.Metadata;
using ERP.Entities._0_Systems;

namespace ERP.APIs.Contracts.Services
{

    public class ContractService : BaseService<EmploymentContract>, IContract
    {
        private readonly ILogger<ContractService> _logger;
        private readonly FileHelper _fileHelper;
        private readonly ContractAddendumService _contractAddendumService;
        private readonly ContractHistoryService _contractHistoryService;

        public ContractService(IBaseRepository<EmploymentContract> repository, 
         ApplicationDbContext context,
         FileHelper fileHelper,
          IMapper mapper,
          ContractAddendumService contractAddendumService,
          ContractHistoryService contractHistoryService,
        ILogger<ContractService> logger) : base(repository, mapper, context)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _contractAddendumService = contractAddendumService;
            _contractHistoryService = contractHistoryService;
        }
        

        public override async Task<ApiResponse<List<EmploymentContract>>> GetAllAsync(Expression<Func<EmploymentContract, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }

        public override async Task<ApiResponse<EmploymentContract?>> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
        public override async Task<ApiResponse<PagedResult<EmploymentContract>>> GetPagedListAsync(
            SearchRequest<EmploymentContract> searchRequest, params Expression<Func<EmploymentContract, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public override async Task<ApiResponse<EmploymentContract>> AddAsync(EmploymentContract entity)
        {
            var res = await base.AddAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<EmploymentContract>> UpdateAsync(EmploymentContract entity)
        {
            var res = await base.UpdateAsync(entity);
            return res;
        }
        public override async Task<ApiResponse<EmploymentContract>> DeleteAsync(int id)
        {
            var res = await base.DeleteAsync(id);
            return res;
        }
        public override async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
        {
            return await base.DeleteRangeAsync(ids);
        }
        public override async Task<bool> DeleteAllAsync()
        {
            return await base.DeleteAllAsync();
        }

        public async Task<ApiRespone_basic> GetById_custom(int id)
        {
            var ContractE = await _repository.GetByIdAsync(id);
            var ContractAddendumE = _context.ContractAddendums.Where(p => p.contractId == id & p.status != -1).ToList();
            var ContractHistoryE = _context.ContractHistories.Where(p => p.contractId == id & p.status != -1).ToList();
            if(ContractE == null)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "entity does not exists"
                };
            }
            var data = new Contract_datares
            {
                ContractRes = _mapper.Map<ContractRes>(ContractE),
                ContractAddendumRes = _mapper.Map<List<ContractAddendumRes>>(ContractAddendumE),
                ContractHistoryRes = _mapper.Map<List<ContractHistoryRes>>(ContractHistoryE)
            };
            return new ApiRespone_basic
            {
                Success = true,
                Data = data
            };
        }

        public async Task<ApiRespone_basic> AddAsync_custom(Contract_dataset req)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ContractE = _mapper.Map<EmploymentContract>(req.contractDTO);
                if(ContractE.contractCode == null)
                {
                    ContractE.contractCode = await GenerateContractCodeByTypeAsync(ContractE.contractTypeId);
                }
                // await HandleFileUploadAsync(req.FileItem, contractFind); // Nếu cần, xử lý file
                ContractE = await hanldeReject("", ContractE, "");
                var saveContractE = await _repository.AddAsync(ContractE);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiRespone_basic
                {
                    Success = true,
                    Message = "Thêm mới thành công"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Lỗi khi tạo hợp đồng: " + ex.Message
                };
            }
        }

        public async Task<ApiRespone_basic> Reject(RejectModel data)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ContractE = await _repository.GetByIdAsync(data.contractId);
                if(ContractE == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Entity does not exists"
                    };
                }
                ContractE = await hanldeReject(data.rejectReason, ContractE, "R");
                await transaction.CommitAsync();
                return new ApiRespone_basic
                {
                    Success = true
                };
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();

                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Lỗi khi hủy hợp đồng: " + ex.Message
                };
            }
        }

        public async Task<EmploymentContract> hanldeReject(string? cancelReason, EmploymentContract entity, string? mode)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.id == entity.employeeId);

            var statusRejects = await _context.sys_Dmtts
                .Where(x => x.isReject == true && x.ma_ct == "CONTRACT")
                .ToListAsync();

            int statusReject = statusRejects
                .Select(x => (int?)x.status_code)
                .Min() ?? -2;

            bool shouldReject = false;

            if (mode == "R")
            {
                shouldReject = true;
                entity.status = statusReject; // Gán nếu là "R"
            }
            else
            {
                // So sánh entity.status có nằm trong danh sách status_code của statusRejects không
                if (statusRejects.Any(x => x.status_code == entity.status))
                {
                    shouldReject = true;
                    // KHÔNG gán lại entity.status
                }
            }

            if (shouldReject)
            {
                entity.notes = cancelReason;

                if (employee != null)
                {
                    employee.status = -1;
                    _context.Employees.Update(employee);
                }

                var history = new ContractHistory
                {
                    contractId = entity.id,
                    desciption = mode == "R" ? " Chấm dứt hợp đồng" : "Hợp đồng hết hạn",
                };

                await _context.ContractHistories.AddAsync(history);
            }

            return entity;
        }

        #region handle edit
        public async Task<ApiRespone_basic> EditAsync_custom(int id, Contract_dataset req)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var contractFind = await _repository.GetByIdAsync(id);
                if (contractFind == null)
                    return ApiResponse_Failed("Entity does not exist");

                var contractCopy = CopyContract(contractFind);

                

                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.id == req.contractDTO.employeeId);

                var changes = req.contractAddendumDTOs == null
                    ? HandleDirectContractChanges(contractCopy, req.contractDTO, employee)
                    : HandleAddendumContractChanges(contractCopy, req.contractAddendumDTOs, employee);

                if (changes.Any())
                    await SaveContractHistoryAsync(id, changes);
                // Nếu có contractAddendumDTOs, bỏ qua việc xử lý và lưu hợp đồng chính
                if (req.contractAddendumDTOs != null && req.contractAddendumDTOs.Any())
                {
                    // Chỉ xử lý các phụ lục hợp đồng mà không làm gì với hợp đồng chính
                    await HandleContractAddendumsAsync(req.contractAddendumDTOs, id);
                }
                else
                {
                    // Nếu không có contractAddendumDTOs, tiến hành xử lý và lưu hợp đồng
                    _mapper.Map(req.contractDTO, contractFind);
                    await GenerateContractCodeIfNeeded(contractFind);
                    // await HandleFileUploadAsync(req.FileItem, contractFind); // Nếu cần, xử lý file

                    contractFind = await hanldeReject("", contractFind, "");
                    var savedContract = await _repository.UpdateAsync(contractFind);
                    if (savedContract == null)
                        return ApiResponse_Failed("Không thể lưu hợp đồng");
                }

                _context.Employees.Update(employee);


                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return ApiResponse_Success("Sửa thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse_Failed("Lỗi khi lưu hợp đồng: " + ex.Message);
            }
        }


        // Helper Methods
        private EmploymentContract CopyContract(EmploymentContract contract)
        {
            return new EmploymentContract
            {
                positionId = contract.positionId,
                departmentId = contract.departmentId,
                salary = contract.salary,
                endDate = contract.endDate
            };
        }

        private async Task GenerateContractCodeIfNeeded(EmploymentContract contract)
        {
            if (string.IsNullOrEmpty(contract.contractCode))
                contract.contractCode = await GenerateContractCodeByTypeAsync(contract.contractTypeId);
        }

        private async Task HandleFileUploadAsync(IFormFile file, EmploymentContract contract)
        {
            if (file != null)
            {
                var result = await _fileHelper.SaveFileAsync(file, "Contract");
                if (!result.Success)
                    throw new Exception("Lỗi khi lưu file: " + result.ErrorMessage);

                contract.filePath = result.FileUrl;
            }
        }

        private async Task HandleContractAddendumsAsync(IEnumerable<ContractAddendumDTO> addendums, int contractId)
        {
            if (addendums != null)
            {
                foreach (var addendum in addendums)
                    addendum.contractId = contractId;

                await _contractAddendumService.ProcessActionTypeListAsync(addendums.ToList());
            }
        }

        private List<ContractChangeItem> HandleDirectContractChanges(EmploymentContract oldContract, ContractDTO newContract, Employee employee)
        {
            var changes = new List<ContractChangeItem>();

            CompareAndAddChange(changes, "Phòng ban", oldContract.departmentId?.ToString(), newContract.departmentId?.ToString(), () => employee.departmentId = newContract.departmentId);
            CompareAndAddChange(changes, "Chức vụ", oldContract.positionId?.ToString(), newContract.positionId?.ToString(), () => employee.positionId = newContract.positionId);
            CompareAndAddChange(changes, "Lương", oldContract.salary.ToString(), newContract.salary.ToString(), () => employee.netSalary = newContract.salary);
            CompareAndAddChange(changes, "Ngày kết thúc hợp đồng", oldContract.endDate.ToString(), newContract.endDate.ToString());

            return changes;
        }

        private List<ContractChangeItem> HandleAddendumContractChanges(EmploymentContract oldContract, IEnumerable<ContractAddendumDTO> addendums, Employee employee)
        {
            var changes = new List<ContractChangeItem>();

            foreach (var addendum in addendums)
            {
                switch (addendum.changeField)
                {
                    case "Department":
                        CompareAndAddChange(changes, "Phòng ban", oldContract.departmentId?.ToString(), addendum.departmentId?.ToString(), () => employee.departmentId = addendum.departmentId);
                        break;
                    case "Position":
                        CompareAndAddChange(changes, "Chức vụ", oldContract.positionId?.ToString(), addendum.positionId?.ToString(), () => employee.positionId = addendum.positionId);
                        break;
                    case "Salary":
                        CompareAndAddChange(changes, "Lương", oldContract.salary.ToString(), addendum.salary.ToString(), () => employee.netSalary = addendum.salary ?? 0);
                        break;
                    case "EndDate":
                        CompareAndAddChange(changes, "Ngày kết thúc hợp đồng", oldContract.endDate.ToString(), addendum.endDate.ToString());
                        break;
                }
            }

            return changes;
        }

        private void CompareAndAddChange(List<ContractChangeItem> changes, string fieldName, string oldValue, string newValue, Action updateAction = null)
        {
            if (oldValue != newValue)
            {
                changes.Add(new ContractChangeItem
                {
                    fieldName = fieldName,
                    oldValue = oldValue,
                    newValue = newValue
                });

                updateAction?.Invoke();
            }
        }

        private async Task SaveContractHistoryAsync(int contractId, List<ContractChangeItem> changes)
        {
            var history = new ContractHistory
            {
                contractId = contractId,
                oldValue = JsonConvert.SerializeObject(changes.Select(c => new { Field = c.fieldName, Value = c.oldValue })),
                newValue = JsonConvert.SerializeObject(changes.Select(c => new { Field = c.fieldName, Value = c.newValue })),
                desciption = "Cập nhật hợp đồng ở trạng thái nháp/chưa hiệu lực"
            };
            await _contractHistoryService.AddAsync(history);
        }

        private ApiRespone_basic ApiResponse_Success(string message)
        {
            return new ApiRespone_basic
            {
                Success = true,
                Message = message
            };
        }

        private ApiRespone_basic ApiResponse_Failed(string message)
        {
            return new ApiRespone_basic
            {
                Success = false,
                Message = message
            };
        }
        #endregion

        public Task<ApiRespone_basic> DeleteAsync_custom(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<string> GenerateContractCodeByTypeAsync(int? contractTypeId = 1)
        {
            var yearSuffix = DateTime.Now.Year.ToString().Substring(2); 
            var contractType = await _context.ContractTypes
                .Where(ct => ct.id == contractTypeId)
                .Select(ct => ct.contractTypeCode) 
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(contractType))
            {
                throw new Exception("Không tìm thấy loại hợp đồng.");
            }
            var prefix = $"HD{yearSuffix}-{contractType}-"; 

            var latestCode = await _context.EmploymentContracts
                .Where(c => c.contractCode.StartsWith(prefix))
                .OrderByDescending(c => c.contractCode)
                .Select(c => c.contractCode)
                .FirstOrDefaultAsync();

            int newNumber = 1;
            if (!string.IsNullOrEmpty(latestCode))
            {
                var parts = latestCode.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out var parsed))
                {
                    newNumber = parsed + 1;
                }
            }
            var newCode = prefix + newNumber.ToString("D4");

            while (await _context.EmploymentContracts.AnyAsync(c => c.contractCode == newCode))
            {
                newNumber++;
                newCode = prefix + newNumber.ToString("D4");
            }

            return newCode;
        }

        public async Task<ApiRespone_basic> GetNew()
        {
            var default_phCode = await GenerateContractCodeByTypeAsync(1);
            return new ApiRespone_basic
            {
                Success = true,
                Data = new
                {
                    default_phCode
                }
            };
        }
    }

}
