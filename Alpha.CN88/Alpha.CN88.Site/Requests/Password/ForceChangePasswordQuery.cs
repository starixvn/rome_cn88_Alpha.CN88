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
            //NickName = viewer.NickName;
            IsSubAccount = viewer.IsSubAcc;
            //SubUserName = viewer.SubUserName;
        }

        //public string NickName { get; }

        public bool IsSubAccount { get; }

        //public string SubUserName { get; }
    }
}