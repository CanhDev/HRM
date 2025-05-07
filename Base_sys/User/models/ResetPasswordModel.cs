namespace ERP.Base_sys.User.models
{
    public class ResetPasswordModel
    {
        public string? UserId { get; set; }
        public string? ResetToken { get; set; }
        public string? NewPassword { get; set; }
    }
}
