namespace Alpha.CN88.Site.Requests.Password
{
    using CN88.Models;
    using MediatR;
    using ViewModels.Password;

    public class ForceChangePasswordQuery : BaseQuery, IRequest<ForceChangePasswordViewModel>
    {
        public ForceChangePasswordQuery(IUser viewer)
            : base(viewer)
        {
            IsSubAccount = viewer.IsSubAcc;
            NickName = viewer.NickName;
            SubUserName = viewer.SubUserName;
        }

        public bool IsSubAccount { get; }

        public string NickName { get; }

        public string SubUserName { get; }
    }
}