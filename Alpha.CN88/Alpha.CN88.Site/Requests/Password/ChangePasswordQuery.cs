namespace Alpha.CN88.Site.Requests.Password
{
    using global::CN88.Auth.Models;
    using MediatR;
    using ViewModels.Password;

    public class ChangePasswordQuery : IRequest<ChangePasswordViewModel>
    {
        public ChangePasswordQuery()
        {
        }

        public ChangePasswordQuery(SecurityResult result)
        {
            Result = result;
        }

        public SecurityResult Result { get; }
    }
}