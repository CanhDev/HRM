using ERP.Base_sys.Login.model;
using ERP.Entities._1_Configs;
using ERP.Entities.Lists.Employee;

namespace ERP.Base_sys.Login.Service
{
    public interface IAccountRepo
    {
        public Task<bool> CreateAccount(ApplicationUser userAccount, string password);
        public Task<ApplicationUser?> CheckAccount(LoginModel model);
    }
}
