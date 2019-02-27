namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;
    using ViewModels.SecurityCode;

    public class ForceChangeSecurityCodeQuery : BaseQuery, IRequest<ForceChangeSecurityCodeViewModel>
    {
        public ForceChangeSecurityCodeQuery(IUser viewer)
            : base(viewer)
        {
            IsForcedSecurityCode = viewer.IsForcedSecurityCode;
            SecurityCode = viewer.SecurityCode;
        }

        public bool IsForcedSecurityCode { get; }

        public string SecurityCode { get; }

        public SecurityResult Result { get; set; }
    }
}