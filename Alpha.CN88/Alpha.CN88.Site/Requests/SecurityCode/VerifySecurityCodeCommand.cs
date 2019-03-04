namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class VerifySecurityCodeCommand : BaseQuery, IRequest<SecurityResult>
    {
        public VerifySecurityCodeCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string SecurityCode { get; set; }

        public string LanguageKey { get; }
    }
}