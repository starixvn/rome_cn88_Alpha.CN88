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
            IsInternal = viewer.IsInternal;
        }

        public bool IsInternal { get; }

        public SecurityResult Result { get; set; }
    }
}