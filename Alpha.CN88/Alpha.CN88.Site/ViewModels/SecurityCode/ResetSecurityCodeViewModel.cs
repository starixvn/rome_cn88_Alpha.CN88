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
            SecurityResult result,
            bool isDisableSaveButton)
        {
            CustId = custId;
            CustName = custName;
            HashKey = hashKey;
            Result = result;
            IsDisableSaveButton = isDisableSaveButton;
        }

        public int CustId { get; }

        public string CustName { get; }

        public string HashKey { get; }

        public SecurityResult Result { get; }

        public bool IsDisableSaveButton { get; }

        public string MainRootPath => UrlHelper.MapMainPath("~");
    }
}