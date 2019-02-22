namespace CN88.Auth.Services
{
    using System;
    using Configurations;
    using Criteria;
    using Criteria.Authentication;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models;
    using Params.Authentication;

    public interface AuthenticationService
    {
        AuthResult Login(string requestParam, string ibckeyPath);

        AuthResult AdminLogin(string requestParam, string ibckeyPath);

        void Logout(LogoutParam logoutParam);

        bool IsDisabledCheckDuplicationLogin { get; }

        bool IsKickedOut(int custId, int onlineUserId);

        bool IsActiveCustomer(int custId, int subAccId);
    }

    public class AuthenticationServiceImpl : AuthenticationService
    {
        private readonly object lockStatus = new object();

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

        public bool IsDisabledCheckDuplicationLogin
            => AuthAppSettings.DisabledCheckDuplicationLogin;

        public bool IsKickedOut(int custId, int onlineUserId)
        {
            var command = new ReCheckinCommand(onlineUserId);
            dynamicRepository.Execute(command);

            return command.IsKickedOut;
        }

        public bool IsActiveCustomer(int custId, int subAccId)
        {
            lock (lockStatus)
            {
                var currentCustomerStatus = sessionUpdater.CurrentCustomerStatus;

                if (currentCustomerStatus != null)
                {
                    DateTime result;
                    var sessionInfo = currentCustomerStatus.Split('_');

                    if (sessionInfo.Length > 1
                        && DateTime.TryParse(sessionInfo[1], out result)
                        && result >= DateTime.Now)
                    {
                        return true;
                    }
                }

                var command = new CheckActiveCustomerCommand(custId, subAccId);
                dynamicRepository.Execute(command);

                if (command.IsActive)
                {
                    sessionUpdater.CurrentCustomerStatus = $"True_{DateTime.Now.AddSeconds(AuthAppSettings.CustomerStatusExpireTime)}";
                }

                return command.IsActive;
            }
        }
    }
}
