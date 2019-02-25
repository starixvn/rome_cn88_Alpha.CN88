namespace Alpha.CN88.Site.Requests.Password
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;
    using ViewModels.Password;

    public class ForceChangePasswordQuery : BaseQuery, IRequest<ForceChangePasswordViewModel>
    {
        public ForceChangePasswordQuery(IUser viewer)
            : base(viewer)
        {
            IsSubAccount = viewer.IsSubAcc;
        }

        public bool IsSubAccount { get; }

        public SecurityResult Result { get; set; }
    }
}