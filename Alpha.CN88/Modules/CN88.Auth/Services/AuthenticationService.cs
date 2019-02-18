namespace CN88.Auth.Services
{
    using System;
    using Criteria;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models;

    public interface AuthenticationService
    {
        AuthResult Login(string requestParam, string ibckeyPath);

        AuthResult AdminLogin(string requestParam, string ibckeyPath);

        void Logout(LogoutParam logoutParam);
    }

    public class AuthenticationServiceImpl : AuthenticationService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly AuthenticationDataAccess authenticationDataAccess;
        private readonly LoginParamParser loginParamParser;
        private readonly SessionUpdater sessionUpdater;

        public AuthenticationServiceImpl(
            IDynamicRepository dynamicRepository,
            AuthenticationDataAccess authenticationDataAccess,
            LoginParamParser loginParamParser,
            SessionUpdater sessionUpdater)
        {
            this.dynamicRepository = dynamicRepository;
            this.authenticationDataAccess = authenticationDataAccess;
            this.loginParamParser = loginParamParser;
            this.sessionUpdater = sessionUpdater;
        }

        public AuthResult Login(string requestParam, string ibckeyPath)
        {
            var keyValuePairs = loginParamParser.DecryptString(requestParam, ibckeyPath);
            var loginParam = loginParamParser.ToObject(keyValuePairs);

            var criteria = new LoginCommand(
                loginParam.CustId,
                loginParam.Password,
                loginParam.SubAccId,
                loginParam.LoginTime,
                loginParam.NickName,
                loginParam.IsNickName,
                loginParam.LanguageKey,
                loginParam.RequestInfo.LoginIP,
                loginParam.RequestInfo.LoginDomain,
                loginParam.RequestInfo.BrowserName,
                loginParam.RequestInfo.Platform,
                loginParam.RequestInfo.CountryCode);

            var outputParams = authenticationDataAccess.ExecuteNonQuery(criteria);
            var errorCode = Convert.ToInt32(outputParams["errcode"]);

            return errorCode != 0
                ? new AuthResult()
                : sessionUpdater.Update(loginParam, outputParams);
        }

        public AuthResult AdminLogin(string requestParam, string ibckeyPath)
        {
            var keyValuePairs = loginParamParser.DecryptString(requestParam, ibckeyPath);
            var loginParam = loginParamParser.ToAdminObject(keyValuePairs);

            var criteria = new AdminLoginCommand(
                loginParam.CustName,
                loginParam.AdminId,
                loginParam.LoginTime,
                loginParam.LanguageKey,
                loginParam.RequestInfo.LoginIP,
                loginParam.RequestInfo.LoginDomain,
                loginParam.RequestInfo.BrowserName,
                loginParam.RequestInfo.Platform,
                loginParam.RequestInfo.CountryCode);

            var outputParams = authenticationDataAccess.ExecuteNonQuery(criteria);
            var errorCode = Convert.ToInt32(outputParams["errcode"]);

            return errorCode != 0
                ? new AuthResult()
                : sessionUpdater.UpdateAdmin(loginParam, outputParams);
        }

        public void Logout(LogoutParam logoutParam)
        {
            dynamicRepository.Execute(new LogoutCommand(
                logoutParam.OnlineUserId,
                string.IsNullOrWhiteSpace(logoutParam.SubUserName) ? logoutParam.CustName : logoutParam.SubUserName,
                logoutParam.CustRoleId,
                logoutParam.ServerId,
                logoutParam.NickName));

            sessionUpdater.ClearUserSession();
        }
    }
}
