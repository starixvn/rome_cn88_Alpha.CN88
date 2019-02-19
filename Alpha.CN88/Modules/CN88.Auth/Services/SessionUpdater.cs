namespace CN88.Auth.Services
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Text.RegularExpressions;
    using Criteria;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;

    public interface SessionUpdater
    {
        AuthResult Update(LoginParam loginParam, Hashtable outputParams);

        AuthResult UpdateAdmin(AdminLoginParam loginParam, Hashtable outputParams);

        void ClearUserSession();
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
            var userSession = UserSession.Current;

            outputParams.Add("custid", loginParam.CustId);
            AddToOutputParams(
                loginParam.CustId,
                loginParam.SubAccId,
                loginParam.RequestInfo.LoginIP,
                userSession.NetSessionId,
                outputParams);

            userSession.UserLoginIp = loginParam.RequestInfo.LoginIP;
            SetUserSessionValues(userSession.NetSessionId, outputParams);

            return new AuthResult(
                loginParam.CustId,
                outputParams["username"].ToString(),
                loginParam.SubAccId,
                userSession.SubUserName,
                loginParam.DeviceId);
        }

        public AuthResult UpdateAdmin(AdminLoginParam loginParam, Hashtable outputParams)
        {
            var userSession = UserSession.Current;

            var custId = Convert.ToInt32(outputParams["custid"]);
            outputParams.Add("username", loginParam.CustName);
            AddToOutputParams(
                custId,
                0,
                loginParam.RequestInfo.LoginIP,
                userSession.NetSessionId,
                outputParams,
                loginParam.AdminId);

            userSession.IsInternalWebVPN = false;
            SetUserSessionValues(userSession.NetSessionId, outputParams);

            return new AuthResult(custId, loginParam.CustName);
        }

        public void ClearUserSession()
            => UserSessionService.RemoveUserSession(UserSession.Current);

        private void AddToOutputParams(
            int custId,
            int subAccId,
            string ip,
            string sessionId,
            Hashtable outputParams,
            int adminId = 0)
        {
            var roleId = Convert.ToByte(outputParams["roleid"]);
            outputParams.Add("sitename", AppSettings.SiteNameFromRoleId(roleId));

            var subAccName = string.Empty;

            if (subAccId != 0)
            {
                subAccName = outputParams["subusername"].ToString().ToUpperInvariant();
                outputParams.Remove("subusername");
            }

            outputParams.Add("subaccid", subAccId);
            outputParams.Add("subaccname", subAccName);

            var serverId = Convert.ToInt32(outputParams["serverid"]);
            outputParams["on_userid"] = GetOnlineUserId(custId, adminId > 0 ? -adminId : subAccId, sessionId, serverId, ip);

            outputParams.Add("adminid", adminId);

            AddCustomerStatusToOutputParams(outputParams, custId, adminId);
        }

        private void AddCustomerStatusToOutputParams(Hashtable outputParams, int custId, int adminId, string systemLogin = "NAP")
        {
            var dataTable = authenticationDataAccess.ExecuteDataTable(
                new GetCustomerStatusCriteria(custId, adminId, systemLogin));

            if (dataTable.Rows.Count > 0)
            {
                var drow = dataTable.Rows[0];

                foreach (DataColumn column in dataTable.Columns)
                {
                    outputParams.Add(column.ColumnName.ToLower(), drow[column]);
                }
            }
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

        private static void SetUserSessionValues(string sessionId, Hashtable outputParams)
        {
            var session = new UserSessionBase(sessionId);

            session.CustId = (int)outputParams["custid"];
            session.UserName = (outputParams["username"] as string).ToUpperInvariant().Trim();
            session.SubAccId = (int)outputParams["subaccid"];

            session.SubUserName = outputParams["subaccname"] as string;
            session.SubPageAccess = (PageAccess)(outputParams["subpageaccess"] as string);

            if (!session.IsSubAcc)
            {
                session.SubPageAccess.SetFullPermission();
            }

            session.SiteName = outputParams["sitename"] as string;
            session.Site = outputParams["site"] as string;

            session.Currency = outputParams["currency"] as string;
            session.CurrencyId = (int)outputParams["currencyid"];
            session.RoleId = Convert.ToByte(outputParams["roleid"]);
            session.ServerId = (int)outputParams["serverid"];

            session.AdminId = (int)outputParams["adminid"];
            session.AdminName = outputParams["accusername"] as string;

            session.IsLogged = session.IsInternal || !(bool)outputParams["isfirstlogin"];

            try
            {
                // Security code.
                session.IsForcedSecurityCode = (bool)outputParams["isforcedchangesecuritycode"];
                session.SecurityCode = outputParams["securitycode"] as string;
            }
            catch
            {
            }

            try
            {
                session.LastChangedPassword = (DateTime)outputParams["lastchangedpwddate"];
            }
            catch
            {
                session.LastChangedPassword = DateTime.MinValue;
            }

            try
            {
                DateTime pwdExpiry = (DateTime)outputParams["pwdexpiry"];
                session.IsPasswordExpired = DateTime.Compare(DateTime.Now, pwdExpiry) >= 0;
            }
            catch
            {
                session.IsPasswordExpired = false;
            }

            if (outputParams.Contains("doublecommlosing"))
            {
                session.IsDoubleCommLosing = (bool)outputParams["doublecommlosing"];
            }

            if (outputParams.Contains("nickname"))
            {
                session.NickName = (outputParams["nickname"] + string.Empty).ToUpperInvariant();
            }

            if (outputParams.Contains("nicknameloginonly"))
            {
                session.IsNicknameLoginOnly = !string.IsNullOrEmpty(outputParams["nicknameloginonly"].ToString()) && (bool)outputParams["nicknameloginonly"];
            }

            if (outputParams.Contains("isnickname"))
            {
                session.IsNickname = !string.IsNullOrEmpty(outputParams["isnickname"].ToString()) && (bool)outputParams["isnickname"];
            }

            session.DisplayNickName = session.IsNickname ? session.NickName : session.UserName;

            session.OnUserId = (int)outputParams["on_userid"];

            session.IsDisabledAutoMPT = (bool)outputParams["disableautompt"];
            session.SRecommendId = (int)outputParams["srecommend"];
            session.MRecommendId = (int)outputParams["mrecommend"];
            session.ARecommendId = (int)outputParams["arecommend"];
            session.IsAdminCanViewVNIP = (bool)outputParams["isadmincanviewvnip"];

            if (AppSettings.IsSupportMyanmarOdds)
            {
                session.IsMyanmarOdds = Convert.ToBoolean(outputParams["ismyanmarodds"]);
            }

            if (outputParams.ContainsKey("isimbc"))
            {
                session.IsIMBCCustomer = Convert.ToBoolean(outputParams["isimbc"]);
            }

            session.IsLimitedHostPermisionOnSubAccountFunction = !string.IsNullOrWhiteSpace(AppSettings.SubAccountPermission_LockAdminAccounts)
                && Regex.IsMatch(string.Format(",{0},", AppSettings.SubAccountPermission_LockAdminAccounts), string.Format(",{0},", session.AdminId), RegexOptions.IgnoreCase)
                && session.IsInternal;
        }
    }
}
