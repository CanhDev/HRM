using AutoMapper;
using ERP.APIs.Leaves.DTOs;
using ERP.APIs.Leaves.entity;
using ERP.APIs.Leaves.Services.interfaces;
using ERP.Base_sys;
using ERP.Base_sys.Helpers;
using ERP.Base_sys.Repos;
using ERP.Base_sys.Services;
using ERP.Entities;
using ERP.Services.Lists.interfaces;
using ERP.Services.Lists;
using ERP.Entities.Lists.Employee;
using System.Linq.Expressions;
using ERP.APIs.Leaves.DTOs_Res;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERP.APIs.Leaves.Services
{
    public class LeaveService : BaseService<LeaveRequest>, ILeave
    {
        private readonly LeaveTypeService _leavingTypeService;
        private readonly LeaveRequest_detailsService _detailsService;

        public LeaveService(IBaseRepository<LeaveRequest> repository,
            ApplicationDbContext context,
            LeaveTypeService leavingTypeService,
            LeaveRequest_detailsService detailsService,
            IMapper mapper ) : base(repository, mapper, context)
        {
            _leavingTypeService = leavingTypeService;
            _detailsService = detailsService;
        }

        public override async Task<ApiResponse<List<LeaveRequest>>> GetAllAsync(Expression<Func<LeaveRequest, bool>>? predicate = null)
        {
            return await base.GetAllAsync(predicate);
        }
        public override async Task<ApiResponse<PagedResult<LeaveRequest>>> GetPagedListAsync(
            SearchRequest<LeaveRequest> searchRequest, params Expression<Func<LeaveRequest, object>>[] includes)
        {
            var response = await base.GetPagedListAsync(searchRequest, includes);
            return response;
        }
        public async Task<ApiRespone_basic> GetNew(int employeeId)
        {
            var balanceLeave = await _context.employeeLeaveBalances
                .Where(p => p.employeeId == employeeId & p.status != -1)
                .Select(x => new
                {
                    x.usedDaysMonth,
                    x.remainingDaysMonth,
                    x.maxDayMonth,
                })
                .ToListAsync();
            var default_phCode = await GenerateVoucherCode();
            return new ApiRespone_basic
            {
                Success = true,
                Data = new
                {
                    balanceLeave,
                    default_phCode
                }
            };
        }
        public async Task<ApiRespone_basic> GetById_custom(int id)
        {
            try
            {
                var LeaveRequestE = await _context.LeaveRequests.FirstOrDefaultAsync(x => x.id == id);
                if (LeaveRequestE == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Entity does not exist"
                    };
                }
                var LeaveRequest_detaisE = await _context.leaveRequest_Details
                    .Where(p => p.leaveRequestId == id & p.status != -1).ToListAsync();
                var LeaveBalance = await _context.employeeLeaveBalances
                    .Where(p => p.employeeId == LeaveRequestE.employeeId & p.status != -1)
                    .Select(x => new
                    {
                        x.usedDaysMonth,
                        x.remainingDaysMonth,
                        x.maxDayMonth,
                    })
                    .ToListAsync();

                var data = new DataResponse_Leave
                {
                    ph = _mapper.Map<LeaveRequestRes>(LeaveRequestE),
                    ct = _mapper.Map<List<LeaveRequestDetailsRes>>(LeaveRequest_detaisE),
                    ctBalance = _mapper.Map<List<LeaveBalanceResSub>>(LeaveBalance)
                };
                return new ApiRespone_basic
                {
                    Success = true,
                    Data = data
                };
            }catch (Exception ex)
            {
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ApiRespone_basic> AddAsync_custom(Dataset_Leave req)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var LeaveRequestE = _mapper.Map<LeaveRequest>(req.ph);
                if(LeaveRequestE.voucher_code == null)
                {
                    LeaveRequestE.voucher_code = await GenerateVoucherCode();
                }
                var statusApproved = _context.sys_Dmtts
                .Where(x => x.isApproved == true & x.ma_ct == "LEAVE")
                .Max(x => x.status_code);
                    LeaveRequestE = saveInfor_approved(LeaveRequestE, LeaveRequestE.employeeId);
                var savedLeaveRequest = await _repository.AddAsync(LeaveRequestE);
                if(req.ct != null && req.ct.Any())
                {
                    var LeaveRequest_detailsE = _mapper.Map<List<LeaveRequest_details>>(req.ct);
                    foreach(var x in LeaveRequest_detailsE)
                    {
                        x.leaveRequestId = savedLeaveRequest.id;
                    }
                   await _context.leaveRequest_Details.AddRangeAsync(LeaveRequest_detailsE);
                }
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
                    Message = "Lỗi khi tạo phiếu: " + ex.Message
                };
            }
        }
        public async Task<ApiRespone_basic> EditAsync_custom(int id, Dataset_Leave req)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var LeaveRequestE = await _context.LeaveRequests.FirstOrDefaultAsync(x => x.id == id);
                if (LeaveRequestE == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Entity does not exist"
                    };
                }
                _mapper.Map(req.ph, LeaveRequestE);
                var statusApproved = _context.sys_Dmtts
                .Where(x => x.isApproved == true & x.ma_ct == "LEAVE")
                .Max(x => x.status_code);
                LeaveRequestE = saveInfor_approved(LeaveRequestE, LeaveRequestE.employeeId);
               var savedLeaveRequest = await _repository.UpdateAsync(LeaveRequestE);
                if (req.ct != null)
                {
                    foreach (var x in req.ct)
                    {
                        x.leaveRequestId = savedLeaveRequest.id;
                    }

                    await _detailsService.ProcessActionTypeListAsync(req.ct.ToList());
                }
                await _context.SaveChangesAsync();
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
                    Message = "Lỗi khi sửa phiếu: " + ex.Message
                };
            }
        }
        public Task<ApiRespone_basic> DeleteAsync_custom(int id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<ApiRespone_basic> Approve(int id, int ApprovedId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var LeaveRequest = await _repository.GetByIdAsync(id);
                if (LeaveRequest == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Entity does not exists"
                    };
                }
                LeaveRequest = saveInfor_approved(LeaveRequest, ApprovedId, "", "A");
                var balance = await _context.employeeLeaveBalances.FirstOrDefaultAsync(p => p.employeeId == LeaveRequest.employeeId);
                if (balance != null)
                {
                    balance.usedDays += LeaveRequest.totalDays;
                    balance.usedDaysMonth += LeaveRequest.totalDays;
                    balance.remainingDays -= LeaveRequest.totalDays;
                    balance.remainingDaysMonth -= LeaveRequest.totalDays;
                }
                var history = new LeaveBalanceHistory
                {
                    employeeId = LeaveRequest.employeeId,
                    daysChanged = LeaveRequest.totalDays,
                    changeDate = LeaveRequest.createdAt,
                    changeBy = ApprovedId,
                    reason = "Duyệt đơn nghỉ phép",
                    mode = 1
                };
                await _context.leaveBalanceHistories.AddAsync(history);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ApiRespone_basic
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Lỗi khi duyệt phiếu: " + ex.Message
                };
            }
        }

        public LeaveRequest saveInfor_approved(LeaveRequest entity, int ApprovedId, string rejectReason = "", string mode = "")
        {
            int statusApproved = _context.sys_Dmtts
                .Where(x => x.isApproved == true && x.ma_ct == "LEAVE")
                .Select(x => (int?)x.status_code)
                .Max() ?? 2;

            int statusReject = _context.sys_Dmtts
                .Where(x => x.isReject == true && x.ma_ct == "LEAVE")
                .Select(x => (int?)x.status_code)
                .Max() ?? 3;

            entity.voucher_date = DateTime.UtcNow;

            switch (mode)
            {
                case "A":
                    entity.approvalStatus = statusApproved;
                    entity.approvalUser = ApprovedId;
                    break;

                case "R":
                    entity.approvalStatus = statusReject;
                    entity.approvalUser = ApprovedId;
                    entity.rejectReason = rejectReason;
                    break;

                default:
                    if (entity.approvalStatus == statusApproved)
                    {
                        entity.approvalStatus = statusApproved;
                        entity.approvalUser = ApprovedId;
                    }
                    else if (entity.approvalStatus == statusReject)
                    {
                        entity.approvalStatus = statusReject;
                        entity.approvalUser = ApprovedId;
                        entity.rejectReason = rejectReason;
                    }
                    break;
            }

            return entity;
        }


        public async Task<ApiRespone_basic> Reject(int id, int ApprovedId, string rejectReason)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var LeaveRequest = await _repository.GetByIdAsync(id);
                if (LeaveRequest == null)
                {
                    return new ApiRespone_basic
                    {
                        Success = false,
                        Message = "Entity does not exists"
                    };
                }
                LeaveRequest = saveInfor_approved(LeaveRequest, ApprovedId, LeaveRequest.rejectReason, "R");
                var history = new LeaveBalanceHistory
                {
                    employeeId = LeaveRequest.employeeId,
                    daysChanged = 0,
                    changeDate = LeaveRequest.createdAt,
                    changeBy = ApprovedId,
                    reason = "Từ chối đơn nghỉ phép",
                    mode = -1
                };
                await _context.leaveBalanceHistories.AddAsync(history);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new ApiRespone_basic
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ApiRespone_basic
                {
                    Success = false,
                    Message = "Lỗi khi từ chối phiếu: " + ex.Message
                };
            }
        }
        private async Task<string> GenerateVoucherCode(string prefix = "LEF")
        {
            var now = DateTime.Now;
            var yearMonth = now.ToString("yyMM");
            var lastCode = await _context.LeaveRequests
                .Where(x => x.voucher_code.StartsWith($"001-{prefix}-{yearMonth}"))
                .OrderByDescending(x => x.voucher_code)
                .Select(x => x.voucher_code)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(lastCode))
            {
                var parts = lastCode.Split('-');
                if (parts.Length == 4 && int.TryParse(parts[3], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"001-{prefix}-{yearMonth}-{nextNumber:D4}";
        }

    }
}
