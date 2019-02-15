namespace CN88.Auth.Services
{
    using System.Threading.Tasks;
    using Criteria;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models;

    public interface AuthenticationService
    {
        AuthResult Login(string requestParam, string ibckeyPath);

        Task LogoutAsync(
           int onlineUserId,
           string userName,
           byte roleId,
           int serverId,
           string nickName);
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

            var criteria = new LoginCriteria(
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

            if (criteria.ErrorCode != 0)
            {
                new AuthResult();
            }

            return sessionUpdater.Update(loginParam, outputParams);
        }

        public async Task LogoutAsync(
            int onlineUserId,
            string userName,
            byte roleId,
            int serverId,
            string nickName)
            => await dynamicRepository.ExecuteAsync(
                new LogoutCriteria(onlineUserId, userName, roleId, serverId, nickName));
    }
}
