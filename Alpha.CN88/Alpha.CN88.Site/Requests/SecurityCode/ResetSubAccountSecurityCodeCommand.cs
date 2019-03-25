namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class ResetSubAccountSecurityCodeCommand : BaseQuery, IRequest<SecurityResult>
    {
        public ResetSubAccountSecurityCodeCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string LanguageKey { get; }

        public string SecurityCode { get; set; }

        public IUser GetViewer()
            => viewer;

        public override bool IsValid()
            => base.IsValid() && !viewer.IsInternal && CustId != ViewerId;
    }
}