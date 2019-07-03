namespace Alpha.CN88.Site.ViewModels.Password
{
    using CN88.Models;
    using global::CN88.Auth.Models;

    public class ResetPasswordViewModel
    {
        public ResetPasswordViewModel(
            int custId,
            string custName,
            UserLevel custLevel,
            string hashKey,
            SecurityResult result)
        {
            CustId = custId;
            CustName = custName;
            CustLevel = custLevel;
            HashKey = hashKey;
            Result = result;
        }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public string HashKey { get; }

        public SecurityResult Result { get; }
    }
}