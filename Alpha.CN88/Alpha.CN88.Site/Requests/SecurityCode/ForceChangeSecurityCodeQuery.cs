namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;
    using MediatR;
    using ViewModels.SecurityCode;

    public class ForceChangeSecurityCodeQuery : BaseQuery, IRequest<ForceChangeSecurityCodeViewModel>
    {
        public ForceChangeSecurityCodeQuery(IUser viewer)
            : base(viewer)
        {
            IsSubAccount = viewer.IsSubAcc;
            SubUserName = viewer.SubUserName;
            NickName = viewer.NickName;
        }

        public bool IsSubAccount { get; }

        public string SubUserName { get; }

        public string NickName { get; }
    }
}