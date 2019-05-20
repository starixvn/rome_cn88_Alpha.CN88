namespace Alpha.CN88.Site.ViewModels.SecurityCode
{
    using Helpers;

    public class VerifySecurityCodeViewModel
    {
        public VerifySecurityCodeViewModel(
            string securityCodeToken,
            string securitySalt)
        {
            SecurityCodeToken = securityCodeToken;
            SecuritySalt = securitySalt;
        }

        public string SecurityCodeToken { get; }

        public string SecuritySalt { get; }

        public string MainRootPath => UrlHelper.MapMainPath("~");
    }
}