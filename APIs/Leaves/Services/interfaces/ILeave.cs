using ERP.APIs.Leaves.DTOs;
using ERP.APIs.Leaves.entity;
using ERP.Base_sys;
using ERP.Base_sys.Services;

namespace ERP.APIs.Leaves.Services.interfaces
{
    public interface ILeave : IBaseService<LeaveRequest>
    {
        Task<ApiRespone_basic> GetNew(int employeeId);
        Task<ApiRespone_basic> GetById_custom(int id);
        Task<ApiRespone_basic> AddAsync_custom(Dataset_Leave req);
        Task<ApiRespone_basic> EditAsync_custom(int id, Dataset_Leave req);
        Task<ApiRespone_basic> DeleteAsync_custom(int id);
        //
        Task<ApiRespone_basic> Approve(int id, int employeeId);
        Task<ApiRespone_basic> Reject(int id, int employeeId, string rejectReason);
    }
}
