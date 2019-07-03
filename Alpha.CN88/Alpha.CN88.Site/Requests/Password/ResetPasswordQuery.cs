namespace Alpha.CN88.Site.Requests.Password
{
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;
    using ViewModels.Password;

    public class ResetPasswordQuery : BaseQuery, IRequest<ResetPasswordViewModel>
    {
        public ResetPasswordQuery(IUser viewer)
            : base(viewer)
        {
        }

        public SecurityResult Result { get; set; }
    }
}