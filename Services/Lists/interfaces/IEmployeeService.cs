using ERP.Base_sys;
using ERP.Base_sys.Services;
using ERP.Data_res.Lists;
using ERP.DTO.Lists;
using ERP.Entities.Lists.Employee;
using System.Linq.Expressions;

namespace ERP.Services.Lists.interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        Task<ApiRespone_basic> GetAllById(int id);
        Task<ApiRespone_basic> AddAsync_full(Employee_dataset req);
        Task<ApiRespone_basic> EditAsync_full(int id, Employee_dataset req);
        Task<ApiRespone_basic> DeleteAsync_full(int id);
        Task<string> GenerateEmployeeCodeWithYearAsync();
    }
}