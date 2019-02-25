namespace Alpha.CN88.Site.ViewModels.Password
{
    using CN88.Models;
    using global::CN88.Auth.Models;

    public class ForceChangePasswordViewModel
    {
        public ForceChangePasswordViewModel(
            int custId,
            int subAccId,
            byte custRoleId,
            string hashKey,
            SecurityResult result = null)
        {
            CustId = custId;
            SubAccountId = subAccId;
            CustRoleId = custRoleId;
            CustRoleLevel = Enumeration.FromValue<UserLevel>(custRoleId);
            HashKey = hashKey;
            ErrorMessage = result?.Message;
        }

        public int CustId { get; }

        public int SubAccountId { get; }

        public byte CustRoleId { get; }

        public UserLevel CustRoleLevel { get; }

        public string HashKey { get; }

        public bool IsChangeMemberPassword => CustRoleLevel.IsMemberLevel();

        public string ErrorMessage { get; }
    }
}