namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using MediatR;
    using ViewModels.SecurityCode;

    public class VerifySecurityCodeQuery : BaseQuery, IRequest<VerifySecurityCodeViewModel>
    {
        public VerifySecurityCodeQuery(IUser viewer)
            : base(viewer)
        {
        }

        public string SecurityCode { get; set; }
    }
}