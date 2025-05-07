using ERP.Base_sys.jwtService;
using ERP.Base_sys.Login.model;

namespace ERP.Base_sys.Login.Service
{
    public interface IAccountService
    {
        public Task<ApiRespone_basic> SignUpAsync(SignUpModel model);
        public Task<ApiRespone_basic> LoginAsync(LoginModel model);
        public Task<ApiRespone_basic> RenewTokenAsync(TokenModel model);
    }
}
