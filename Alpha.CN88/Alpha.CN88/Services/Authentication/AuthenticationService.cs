namespace Alpha.CN88.Services.Authentication
{
    using System;
    using System.Collections;
    using Models.Authentication;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;

    public interface AuthenticationService
    {
        AuthResult Login(
            string requestParam,
            string ibckeyPath,
            Func<string, Hashtable> getRequestInfo,
            string userLoginIp);

        AuthResult AdminLogin(
            string requestParam,
            string ibckeyPath,
            Func<string, Hashtable> getRequestInfo);

        bool Logout();
    }

    public class AuthenticationServiceImpl : AuthenticationService
    {
        private readonly LoginParamParser loginParamParser;
        private readonly IUserSession userSession;
        private readonly AuthorizationBO authorizationBO;

        public AuthenticationServiceImpl(LoginParamParser loginParamParser, IUserSession userSession)
        {
            this.loginParamParser = loginParamParser;
            this.userSession = userSession;
            authorizationBO = new AuthorizationBO(userSession.NetSessionId);
        }

        public AuthResult Login(
            string requestParam,
            string ibckeyPath,
            Func<string, Hashtable> getRequestInfo,
            string userLoginIp)
        {
            var keyValuePairs = loginParamParser.ToStringArray(requestParam, ibckeyPath);

            if (keyValuePairs.Length == 0)
            {
                return new AuthResult(1001, "Cannot verify request key ");
            }

            var loginParam = loginParamParser.ToObject(keyValuePairs);

            if ((loginParam.CustId == 0 && loginParam.SubAccId == 0) || loginParam.TimeTicks == 0)
            {
                return new AuthResult(1002, "Verify customer information failed. " + string.Join(";", keyValuePairs));
            }

            int errorCode;
            string errorMessage;
            var session = userSession;

            var isSuccess = authorizationBO.AfterSignIn(
                loginParam.CustId,
                loginParam.Password,
                loginParam.SubAccId,
                loginParam.NickName,
                loginParam.IsNickName,
                loginParam.LoginTime,
                getRequestInfo(loginParam.BrowserSize),
                out errorCode,
                out errorMessage,
                ref session,
                (userSession, userData) => { });
            //new UserSessionServiceImpl().SetUserStatusToSession);

            if (!isSuccess)
            {
                return new AuthResult(errorCode, errorMessage);
            }

            session.UserLoginIp = userLoginIp;

            return new AuthResult(
                session.CustId,
                session.UserName,
                session.SubAccId,
                session.SubUserName,
                loginParam.DeviceId);
        }

        public AuthResult AdminLogin(
            string requestParam,
            string ibckeyPath,
            Func<string, Hashtable> getRequestInfo)
        {
            var keyValuePairs = loginParamParser.ToStringArray(requestParam, ibckeyPath);

            if (keyValuePairs.Length == 0)
            {
                return new AuthResult(1001, "Cannot verify request key.");
            }

            var loginParam = loginParamParser.ToAdminObject(keyValuePairs);

            if (loginParam.AdminId == 0
                || string.IsNullOrWhiteSpace(loginParam.CustName)
                || loginParam.TimeTicks == 0)
            {
                return new AuthResult(1002, "Verify customer information failed. " + string.Join(";", keyValuePairs));
            }

            int errorCode;
            string errorMessage;
            var session = userSession;

            var isSuccess = authorizationBO.InternalSignIn(
                loginParam.CustName,
                loginParam.AdminId,
                loginParam.LoginTime,
                getRequestInfo(loginParam.BrowserSize),
                out errorCode,
                out errorMessage,
                ref session,
                (userSession, userData) => { },
                //new UserSessionServiceImpl(systemLogin: systemLogin).SetUserStatusToSession,
                loginParam.SystemLogin);

            if (!isSuccess)
            {
                return new AuthResult(errorCode, errorMessage);
            }

            return new AuthResult(session.CustId, session.UserName);
        }

        public bool Logout()
        {
            authorizationBO.SignOut();

            return true;
        }
    }
}
