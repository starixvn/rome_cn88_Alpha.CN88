namespace CN88.Auth.Services
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Text.RegularExpressions;
    using Constants;
    using Criteria;
    using Criteria.Authentication;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models;
    using Params.Authentication;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;

    public interface SessionUpdater
    {
        AuthResult Update(LoginParam loginParam, Hashtable outputParams);

        AuthResult UpdateAdmin(AdminLoginParam loginParam, Hashtable outputParams);

        void ClearUserSession();

        string CurrentCustomerStatus { get; set; }
    }

    public class SessionUpdaterImpl : SessionUpdater
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly AuthenticationDataAccess authenticationDataAccess;

        public SessionUpdaterImpl(
            IDynamicRepository dynamicRepository,
            AuthenticationDataAccess authenticationDataAccess)
        {
            this.dynamicRepository = dynamicRepository;
            this.authenticationDataAccess = authenticationDataAccess;
        }

        public AuthResult Update(LoginParam loginParam, Hashtable outputParams)
        {
            var userSession = UserSession.Current as UserSessionBase;

            userSession.CustId = loginParam.CustId;
            userSession.UserName = (outputParams["username"] as string).ToUpperInvariant().Trim();
            userSession.Language = loginParam.Language;
            userSession.LangKey = loginParam.LanguageKey;
            userSession.UserLoginIp = loginParam.RequestInfo.LoginIP;

            SetUserSessionValues(
                userSession,
                loginParam.CustId,
                loginParam.RequestInfo.LoginIP,
                outputParams,
                loginParam.SubAccId);

            return new AuthResult(
                loginParam.CustId,
                userSession.UserName,
                loginParam.SubAccId,
                userSession.SubUserName,
                loginParam.DeviceId);
        }

        public AuthResult UpdateAdmin(AdminLoginParam loginParam, Hashtable outputParams)
        {
            var custId = Convert.ToInt32(outputParams["custid"]);

            var userSession = UserSession.Current as UserSessionBase;
            userSession.CustId = custId;
            userSession.UserName = loginParam.CustName.ToUpperInvariant().Trim();
            userSession.Language = loginParam.Language;
            userSession.LangKey = loginParam.LanguageKey;
            userSession.IsInternalWebVPN = false;

            SetUserSessionValues(
               userSession,
               custId,
               loginParam.RequestInfo.LoginIP,
               outputParams,
               adminId: loginParam.AdminId,
               systemLogin: loginParam.SystemLogin);

            return new AuthResult(custId, userSession.UserName, 0, null, "N/A", loginParam.AfterLogInSite, loginParam.AfterLogInQuery);
        }

        public void ClearUserSession()
            => UserSessionService.RemoveUserSession(UserSession.Current);

        public string CurrentCustomerStatus
        {
            get
            {
                return SessionHelper.Get<string>(SessionKeys.CustomerClosedStatus);
            }

            set
            {
                SessionHelper.Add(SessionKeys.CustomerClosedStatus, value);
            }
        }

        private void SetUserSessionValues(
            UserSessionBase userSession,
            int custId,
            string ip,
            Hashtable outputParams,
            int subAccId = 0,
            int adminId = 0,
            string systemLogin = "NAP")
        {
            userSession.SubAccId = subAccId;

            if (subAccId != 0)
            {
                userSession.SubUserName = outputParams["subusername"].ToString().ToUpperInvariant();
                userSession.SubPageAccess = outputParams["subpageaccess"] as string;
            }
            else
            {
                userSession.SubUserName = string.Empty;

                var pageAccess = new PageAccess();
                pageAccess.SetFullPermission();
                userSession.SubPageAccess = pageAccess;
            }

            var roleId = Convert.ToByte(outputParams["roleid"]);
            userSession.RoleId = roleId;
            userSession.SiteName = AppSettings.SiteNameFromRoleId(roleId);
            userSession.Site = outputParams["site"] as string;

            userSession.Currency = outputParams["currency"] as string;
            userSession.CurrencyId = (int)outputParams["currencyid"];

            var serverId = Convert.ToInt32(outputParams["serverid"]);
            userSession.ServerId = serverId;
            userSession.OnUserId = GetOnlineUserId(custId, adminId > 0 ? -adminId : subAccId, userSession.NetSessionId, serverId, ip);

            userSession.AdminId = adminId;
            userSession.AdminName = outputParams["accusername"] as string;

            userSession.IsLogged = userSession.IsInternal || !(bool)outputParams["isfirstlogin"];

            try
            {
                // Security code.
                userSession.IsForcedSecurityCode = (bool)outputParams["isforcedchangesecuritycode"];
                userSession.SecurityCode = outputParams["securitycode"] as string;
            }
            catch
            {
            }

            try
            {
                DateTime pwdExpiry = (DateTime)outputParams["pwdexpiry"];
                userSession.IsPasswordExpired = DateTime.Compare(DateTime.Now, pwdExpiry) >= 0;
            }
            catch
            {
                userSession.IsPasswordExpired = false;
            }

            if (outputParams.Contains("nickname"))
            {
                userSession.NickName = outputParams["nickname"] as string;
            }

            if (outputParams.Contains("nicknameloginonly"))
            {
                userSession.IsNicknameLoginOnly = !string.IsNullOrEmpty(outputParams["nicknameloginonly"] as string) && (bool)outputParams["nicknameloginonly"];
            }

            if (outputParams.Contains("isnickname"))
            {
                userSession.IsNickname = !string.IsNullOrEmpty(outputParams["isnickname"] as string) && (bool)outputParams["isnickname"];
            }

            userSession.DisplayNickName = userSession.IsNickname ? userSession.NickName : userSession.UserName;

            userSession.IsLimitedHostPermisionOnSubAccountFunction = !string.IsNullOrWhiteSpace(AppSettings.SubAccountPermission_LockAdminAccounts)
                && Regex.IsMatch(string.Format(",{0},", AppSettings.SubAccountPermission_LockAdminAccounts), string.Format(",{0},", userSession.AdminId), RegexOptions.IgnoreCase)
                && userSession.IsInternal;

            SetCustomerStatusToUserSession(userSession, custId, adminId, systemLogin);

            AddCustomDataToSession(outputParams);
        }

        private int GetOnlineUserId(
            int custId,
            int subAccId,
            string netSessionId,
            int serverId,
            string ip)
        {
            var command = new CheckinCommand(custId, subAccId, netSessionId, serverId, ip);
            dynamicRepository.Execute(command);

            return command.OnlineUserId;
        }

        private void SetCustomerStatusToUserSession(
            UserSessionBase userSession,
            int custId,
            int adminId = 0,
            string systemLogin = "NAP")
        {
            var dataTable = authenticationDataAccess.ExecuteDataTable(
                new GetCustomerStatusCriteria(custId, adminId, systemLogin));

            if (dataTable.Rows.Count > 0)
            {
                var drow = dataTable.Rows[0];
                DataColumnCollection columns = dataTable.Columns;

                userSession.SRecommendId = Convert.ToInt32(drow["srecommend"]);
                userSession.MRecommendId = Convert.ToInt32(drow["mrecommend"]);
                userSession.ARecommendId = Convert.ToInt32(drow["arecommend"]);
                userSession.IsDisabledAutoMPT = Convert.ToBoolean(drow["disableautompt"]);
                userSession.IsAdminCanViewVNIP = Convert.ToBoolean(drow["isadmincanviewvnip"]);

                if (AppSettings.IsSupportMyanmarOdds)
                {
                    userSession.IsMyanmarOdds = Convert.ToBoolean(drow["ismyanmarodds"]);
                }

                if (columns.Contains("isimbc"))
                {
                    userSession.IsIMBCCustomer = Convert.ToBoolean(drow["isimbc"]);
                }

                if (columns.Contains("doublecommlosing"))
                {
                    userSession.IsDoubleCommLosing = Convert.ToBoolean(drow["doublecommlosing"]);
                }
            }
        }

        private static void AddCustomDataToSession(Hashtable outputParams)
        {
            SessionHelper.Add(SessionKeys.ClearClientData, true);
            SessionHelper.Add(SessionKeys.CustomerStatus, outputParams["status"]);
            SessionHelper.Add(SessionKeys.IsAllDownline, outputParams["isalldownline"]);
        }
    }
}