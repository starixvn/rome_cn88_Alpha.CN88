namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class ChangeSecurityCodeFirstLoginCommand : BaseQuery, IRequest<SecurityResult>
    {
        public ChangeSecurityCodeFirstLoginCommand(IUser viewer)
            : base(viewer)
        {
            IsForcedSecurityCode = viewer.IsForcedSecurityCode;
            SecurityCodeInSession = viewer.SecurityCode;
            LanguageKey = viewer.LanguageKey;
        }

        public bool IsForcedSecurityCode { get; }

        public string SecurityCodeInSession { get; }

        public string LanguageKey { get; }

        public string SecurityCode { get; set; }

        public IUser GetViewer()
            => viewer;
    }
}