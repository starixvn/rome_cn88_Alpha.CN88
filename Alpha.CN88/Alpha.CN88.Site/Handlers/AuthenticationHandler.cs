namespace Alpha.CN88.Site.Handlers
{
    using global::CN88.Auth.Models;
    using global::CN88.Auth.Services;
    using MediatR;
    using Requests.Authentication;

    public class AuthenticationHandler :
        IRequestHandler<LoginQuery, AuthResult>,
        IRequestHandler<AdminLoginQuery, AuthResult>,
        IRequestHandler<LogoutQuery, bool>
    {
        private readonly AuthenticationService authenticationService;

        public AuthenticationHandler(AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public AuthResult Handle(LoginQuery message)
            => authenticationService.Login(message.Params, message.IBCKeyPath);

        public AuthResult Handle(AdminLoginQuery message)
            => authenticationService.AdminLogin(message.Params, message.IBCKeyPath);

        public bool Handle(LogoutQuery message)
        {
            authenticationService.Logout(new LogoutParam(
                message.OnlineUserId,
                message.CustName,
                message.SubUserName,
                message.NickName ?? string.Empty,
                message.CustLevelId,
                message.ServerId));

            return true;
        }
    }
}