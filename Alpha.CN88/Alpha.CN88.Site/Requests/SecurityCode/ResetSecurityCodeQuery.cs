namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;
    using ViewModels.SecurityCode;

    public class ResetSecurityCodeQuery : BaseQuery, IRequest<ResetSecurityCodeViewModel>
    {
        public ResetSecurityCodeQuery(IUser viewer)
            : base(viewer)
        {
        }

        public SecurityResult Result { get; set; }

        public bool IsValidForceChangeFirstLogin()
            => base.IsValid()
            && !viewer.IsInternal
            && viewer.IsForcedSecurityCode
            && string.IsNullOrWhiteSpace(viewer.SecurityCode);
    }
}