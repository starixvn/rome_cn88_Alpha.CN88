namespace Alpha.CN88.Site.Requests.Password
{
    using global::CN88.Auth.Models;
    using MediatR;
    using ViewModels.Password;

    public class ResetPasswordQuery : IRequest<PasswordViewModel>
    {
        public ResetPasswordQuery()
        {
        }

        public ResetPasswordQuery(SecurityResult result)
        {
            Result = result;
        }

        public SecurityResult Result { get; }
    }
}