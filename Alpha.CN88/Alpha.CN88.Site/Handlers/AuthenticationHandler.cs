namespace Alpha.CN88.Site.Handlers
{
    using System;
    using System.Diagnostics;
    using CN88.Models.Authentication;
    using Fanex.Logging.Extensions.Web;
    using MediatR;
    using Requests.Authentication;
    using Services.Authentication;

    public class AuthenticationHandler :
        IRequestHandler<LoginQuery, AuthResult>,
        IRequestHandler<AdminLoginQuery, AuthResult>,
        IRequestHandler<LogoutQuery, bool>
    {
        private readonly LoginParamParser loginParamParser;

        public AuthenticationHandler(LoginParamParser loginParamParser)
        {
            this.loginParamParser = loginParamParser;
        }

        public AuthResult Handle(LoginQuery message)
        {
            var keyValuePairs = loginParamParser.ToStringArray(message.Params, message.IBCKeyPath);

            if (keyValuePairs.Length == 0)
            {
                //ExceptionHandler.Handle(new Exception("Cannot verify request key " + Request.RawUrl));
                return new AuthResult();
            }

            var loginParam = loginParamParser.ToObject(keyValuePairs);

            if ((loginParam.CustId == 0 && loginParam.SubAccId == 0) || loginParam.TimeTicks == 0)
            {
                ExceptionHandler.Handle(new Exception("Verify customer information failed. " + string.Join(";", keyValuePairs)), TraceEventType.Information);
                return new AuthResult();
            }

            return new AuthResult(loginParam.CustId, loginParam.SubAccId, message.CurrentUserSession.UserName);

            //int errorCode;
            //string errorMessage;
            //var session = message.CurrentUserSession;
            //var authorizationBO = new AuthorizationBO(session.NetSessionId);

            //return authorizationBO.AfterSignIn(
            //    loginParam.CustId,
            //    loginParam.Password,
            //    loginParam.SubAccId,
            //    loginParam.NickName,
            //    loginParam.IsNickName,
            //    loginParam.LoginTime,
            //    DeviceHelper.GetRequestInfo(loginParam.BrowserSize),
            //    out errorCode,
            //    out errorMessage,
            //    ref session,
            //    (userSession, userData) => { });
            //    //new UserSessionServiceImpl().SetUserStatusToSession);
        }

        public AuthResult Handle(AdminLoginQuery message)
        {
            throw new NotImplementedException();
        }

        //public bool Handle(AdminLoginQuery message)
        //{
        //    var keyValuePairs = loginParamParser.ToStringArray(message.Params, message.IBCKeyPath);

        //    if (keyValuePairs.Length == 0)
        //    {
        //        return false;
        //    }

        //    var adminLoginParam = loginParamParser.ToAdminObject(keyValuePairs);

        //    if (adminLoginParam.AdminId == 0
        //        || string.IsNullOrWhiteSpace(adminLoginParam.CustName)
        //        || adminLoginParam.TimeTicks == 0)
        //    {
        //        return false;
        //    }

        //    int errorCode;
        //    string errorMessage;
        //    var session = message.CurrentUserSession;
        //    var authorizationBO = new AuthorizationBO(session.NetSessionId);

        //    return authorizationBO.InternalSignIn(
        //        adminLoginParam.CustName,
        //        adminLoginParam.AdminId,
        //        adminLoginParam.LoginTime,
        //        DeviceHelper.GetRequestInfo(adminLoginParam.BrowserSize),
        //        out errorCode,
        //        out errorMessage,
        //        ref session,
        //        (userSession, userData) => { },
        //        //new UserSessionServiceImpl(systemLogin: systemLogin).SetUserStatusToSession,
        //        adminLoginParam.SystemLogin);
        //}

        public bool Handle(LogoutQuery message)
        {
            //var authorizationBO = new AuthorizationBO(message.CurrentUserSession.NetSessionId);
            //authorizationBO.SignOut();

            //return true;
            throw new NotImplementedException();
        }
    }
}