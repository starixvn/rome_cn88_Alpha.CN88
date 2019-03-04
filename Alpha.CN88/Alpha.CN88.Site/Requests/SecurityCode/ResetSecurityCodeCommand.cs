namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class ResetSecurityCodeCommand : BaseQuery, IRequest<SecurityResult>
    {
        public ResetSecurityCodeCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
            IsChangeSecurityCodeFirstLogin = !viewer.IsInternal
                && viewer.IsForcedSecurityCode
                && string.IsNullOrWhiteSpace(viewer.SecurityCode);
        }

        public string LanguageKey { get; }

        public bool IsChangeSecurityCodeFirstLogin { get; }

        public string SecurityCode { get; set; }

        public IUser GetViewer()
            => viewer;

        public bool IsValidChangeSecurityCodeFirstLogin()
            => base.IsValid() && IsChangeSecurityCodeFirstLogin;

        public bool IsValidResetSecurityCode()
            => base.IsValid() && !viewer.IsInternal && CustId != ViewerId;
    }
}