namespace Alpha.CN88.Site.ViewModels.SecurityCode
{
    using global::CN88.Auth.Models;
    using Helpers;

    public class ResetSecurityCodeViewModel
    {
        public ResetSecurityCodeViewModel(
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

        public string MainRootPath
            => UrlHelper.MapMainPath("~");
    }
}