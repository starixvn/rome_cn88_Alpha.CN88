namespace Alpha.CN88.Site.ViewModels.SecurityCode
{
    using global::CN88.Auth.Models;
    using Helpers;

    public class ForceChangeSecurityCodeViewModel
    {
        public ForceChangeSecurityCodeViewModel(
            string hashKey,
            SecurityResult result) : this(null, null, null, hashKey, result)
        {
        }

        public ForceChangeSecurityCodeViewModel(
            string custName,
            string securityCodeToken,
            string securitySalt,
            string hashKey,
            SecurityResult result)
        {
            CustName = custName;
            SecurityCodeToken = securityCodeToken;
            SecuritySalt = securitySalt;
            HashKey = hashKey;
            Result = result;
        }

        public string CustName { get; }

        public string SecurityCodeToken { get; }

        public string SecuritySalt { get; }

        public string HashKey { get; }

        public SecurityResult Result { get; }

        public string MainRootPath
            => UrlHelper.MapMainPath("~");
    }
}