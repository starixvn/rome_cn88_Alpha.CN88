namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;
    using ViewModels.SecurityCode;

    public class ChangeSecurityCodeQuery : BaseQuery, IRequest<ChangeSecurityCodeViewModel>
    {
        public ChangeSecurityCodeQuery(IUser viewer)
            : base(viewer)
        {
            IsForceChangeSecurityCode = !viewer.IsInternal
                && viewer.IsForcedSecurityCode
                && !string.IsNullOrWhiteSpace(viewer.SecurityCode);
        }

        public bool IsForceChangeSecurityCode { get; }

        public SecurityResult Result { get; set; }

        public bool IsValidForceChangeSecurityCode()
            => base.IsValid() && IsForceChangeSecurityCode;
    }
}