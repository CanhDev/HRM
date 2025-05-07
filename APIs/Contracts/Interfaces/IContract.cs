using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.APIs.Contracts.Model;
using ERP.APIs.Contracts.Model.Contract;

namespace ERP.APIs.Contracts.Interfaces
{
    public interface IContract : IBaseService<EmploymentContract>
    {
        Task<ApiRespone_basic> GetNew();
        Task<ApiRespone_basic> GetById_custom(int id);
        Task<ApiRespone_basic> AddAsync_custom(Contract_dataset req);
        Task<ApiRespone_basic> EditAsync_custom(int id, Contract_dataset req);
        Task<ApiRespone_basic> DeleteAsync_custom(int id);
        Task<ApiRespone_basic> Reject(RejectModel data);
    }
}
