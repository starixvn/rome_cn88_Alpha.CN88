namespace CN88.Auth.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class LogoutCriteria : NonQueryCommand
    {
        public LogoutCriteria(
            int onlineUserId,
            string userName,
            byte roleId,
            int serverId,
            string nickname)
        {
            OnlineUserId = onlineUserId;
            UserName = userName;
            RoleId = roleId;
            ServerId = serverId;
            Nickname = nickname;
        }

        [SpParam(SqlParamName = "on_userid")]
        public int OnlineUserId { get; }

        public string UserName { get; }

        public byte RoleId { get; }

        public int ServerId { get; }

        public string Nickname { get; }

        public override string GetSettingKey()
            => "Acc_CheckOut";

        public override bool IsValid()
            => OnlineUserId > 0
            && !string.IsNullOrWhiteSpace(UserName)
            && RoleId > 0
            && ServerId > 0;
    }
}
