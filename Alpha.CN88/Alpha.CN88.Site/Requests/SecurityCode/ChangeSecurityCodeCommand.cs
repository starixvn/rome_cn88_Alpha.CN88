namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class ChangeSecurityCodeCommand : BaseQuery, IRequest<SecurityResult>
    {
        public ChangeSecurityCodeCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
            IsForceChangeSecurityCode = !viewer.IsInternal
                && viewer.IsForcedSecurityCode
                && !string.IsNullOrWhiteSpace(viewer.SecurityCode);
        }

        public string OldSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public string LanguageKey { get; }

        public bool IsForceChangeSecurityCode { get; }

        public IUser GetViewer()
            => viewer;

        public bool IsValidForceChangeSecurityCode()
            => base.IsValid() && IsForceChangeSecurityCode;

        public bool IsValidChangeSecurityCode()
            => base.IsValid() && !viewer.IsInternal;
    }
}