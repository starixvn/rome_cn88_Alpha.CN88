namespace CN88.Auth.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class CheckinCommand : NonQueryCommand
    {
        public CheckinCommand(
            int custId,
            int subAccId,
            string netSessionId,
            int serverId,
            string ip)
        {
            CustId = custId;
            SubAccId = subAccId;
            NetSessionId = netSessionId;
            ServerId = serverId;
            IP = ip;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string NetSessionId { get; }

        public int ServerId { get; }

        public string IP { get; }

        [SpParam(SqlParamName = "on_userid")]
        public int OnlineUserId { get; set; }

        public override string GetSettingKey()
            => "Acc_Checkin";

        public override bool IsValid()
            => CustId > 0
            && !string.IsNullOrWhiteSpace(NetSessionId)
            && ServerId > 0;
    }
}
