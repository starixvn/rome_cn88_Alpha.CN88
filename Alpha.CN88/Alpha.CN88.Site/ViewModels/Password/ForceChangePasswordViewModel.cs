namespace Alpha.CN88.Site.ViewModels.Password
{
    using CN88.Models;

    public class ForceChangePasswordViewModel
    {
        public ForceChangePasswordViewModel(
            int custId,
            int subAccId,
            byte custRoleId,
            string hashKey)
        {
            CustId = custId;
            SubAccountId = subAccId;
            CustRoleId = custRoleId;
            CustRoleLevel = Enumeration.FromValue<UserLevel>(custRoleId);
            HashKey = hashKey;
        }

        public int CustId { get; }

        public int SubAccountId { get; }

        public byte CustRoleId { get; }

        public UserLevel CustRoleLevel { get; }

        public string HashKey { get; }

        public bool IsChangeMemberPassword => CustRoleLevel.IsMemberLevel();
    }
}