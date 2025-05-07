using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities.Vouchers.Employee;
using ERP.DTO.Lists;

namespace ERP.Services.Lists.interfaces
{
    public interface IEmployeeDocumentService : IBaseService<EmployeeDocument>
    {
        Task<ApiRespone_basic> AddAsync_full(List<EmployeeDocumentDTO> req);
        Task<ApiRespone_basic> EditAsync_full(List<EmployeeDocumentDTO> req);
    }
}