namespace Alpha.CN88.Site.ViewModels.Password
{
    using CN88.Models;

    public class PasswordViewModel
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(CustRoleId).DisplayName.ToLower();
            }
        }

        public string HashKey { get; set; }

        public bool IsCasinoSync { get; set; }

        public bool EnableSecurityCode { get; set; }

        public bool IsChangeMemberPassword
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(CustRoleId).IsMemberLevel();
            }
        }

        public string Title { get; set; }
    }
}