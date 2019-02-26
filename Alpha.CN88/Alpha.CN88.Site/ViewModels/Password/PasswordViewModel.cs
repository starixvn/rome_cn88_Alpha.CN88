namespace Alpha.CN88.Site.ViewModels.Password
{
    using global::CN88.Auth.Models;

    public class PasswordViewModel
    {
        public PasswordViewModel(
            int custId,
            string custName,
            string hashKey,
            SecurityResult result)
        {
            CustId = custId;
            CustName = custName;
            HashKey = hashKey;
            Result = result;
        }

        public int CustId { get; }

        public string CustName { get; }

        public string HashKey { get; }

        public SecurityResult Result { get; }
    }
}