namespace Alpha.CN88.Site.Requests.Password
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;
    using ViewModels.Password;

    public class ChangePasswordQuery : BaseQuery, IRequest<ChangePasswordViewModel>
    {
        public ChangePasswordQuery(IUser viewer)
            : base(viewer)
        {
        }

        public SecurityResult Result { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && !viewer.IsInternal
            && (!viewer.IsLogged || viewer.IsPasswordExpired);
    }
}