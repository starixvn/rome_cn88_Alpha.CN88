namespace CN88.Auth.Services
{
    using System;
    using System.Collections;
    using System.Data;
    using Criteria;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Models;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;

    public interface SessionUpdater
    {
        AuthResult Update(LoginParam loginParam, Hashtable outputs);
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

            var roleId = Convert.ToByte(outputParams["roleid"]);
            outputParams.Add("sitename", AppSettings.SiteNameFromRoleId(roleId));

            var isSubAcc = loginParam.SubAccId != 0;
            var subAccName = isSubAcc ? outputParams["subusername"].ToString().ToUpperInvariant() : string.Empty;
            outputParams.Add("subaccid", loginParam.SubAccId);
            outputParams.Add("subaccname", subAccName);

            if (isSubAcc)
            {
                outputParams.Remove("subusername");
            }

            var serverId = Convert.ToInt32(outputParams["serverid"]);
            outputParams.Add("on_userid", GetOnlineUserId(loginParam.CustId, loginParam.SubAccId, userSession.NetSessionId, serverId, loginParam.RequestInfo.LoginIP));

            AddCustomerStatusToOutputParams(outputParams, loginParam.CustId);

            // bool isAllowedTransferTotalBalance = this.dao.Age_Transfer_CheckTotalBalanceTransfer(custId, Sunplus.Agent.Model.BL.Definitions.AppSettings.AllowedListSuperAccounts);
            outputParams.Add("isallowedtotalbalancetransfer", false);

            outputParams.Add("adminid", 0);

            // bool isShowConsolidated = ConfigurationManager.AppSettings["EnableConsolidatedReport"] == "1" && this.IsShownConsilatedReport(custId);
            outputParams.Add("isshowconsolidated", false);

            outputParams.Add("isShowBetChoiceLimit", authenticationDataAccess.IsShowBetChoiceLimit(new CheckBetChoiceLimitCriteria(loginParam.CustId)));

            userSession.UserLoginIp = loginParam.RequestInfo.LoginIP;

            UserSessionService.SetUserSessionValues(userSession, outputParams, AfterUpdate);

            return new AuthResult(
                loginParam.CustId,
                outputParams["username"].ToString(),
                loginParam.SubAccId,
                subAccName,
                loginParam.DeviceId);
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

        private void AddCustomerStatusToOutputParams(
            Hashtable outputParams,
            int custId,
            int adminId = 0,
            string systemLogin = "NAP")
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

        private void AfterUpdate(IUserSession userSession, Hashtable outputParams)
        {
        }
    }
}
