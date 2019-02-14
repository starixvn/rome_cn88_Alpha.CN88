namespace Alpha.CN88.Site.Handlers
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using CN88.Models.Authentication;
    using Fanex.Logging.Extensions.Web;
    using Helpers;
    using MediatR;
    using Requests.Authentication;
    using Services.Authentication;

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
        {
            var result = authenticationService.Login(
                message.Params,
                message.IBCKeyPath,
                DeviceHelper.GetRequestInfo,
                IPHelper.CustomerIp());

            if (result.Code == 1001)
            {
                ExceptionHandler.Handle(new Exception(result.Message + HttpContext.Current.Request.RawUrl));
            }
            else if (result.Code == 1002)
            {
                ExceptionHandler.Handle(new Exception(result.Message), TraceEventType.Information);
            }

            return result;
        }

        public AuthResult Handle(AdminLoginQuery message)
            => authenticationService.AdminLogin(
                message.Params,
                message.IBCKeyPath,
                DeviceHelper.GetRequestInfo);

        public bool Handle(LogoutQuery message)
            => authenticationService.Logout();
    }
}