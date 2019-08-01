namespace Alpha.CN88.Site.ViewModels.Password
{
    using global::CN88.Auth.Models;
    using global::CN88.Web.Models;

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