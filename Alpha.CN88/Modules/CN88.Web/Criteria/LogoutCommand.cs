namespace CN88.Web.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    internal class LogoutCommand: NonQueryCommand
    {
        internal LogoutCommand(LogoutContext logoutContext)
        {
            OnlineUserId = logoutContext.OnlineUserId;
            UserName = logoutContext.UserName;
            RoleId = logoutContext.RoleId;
            ServerId = logoutContext.ServerId;
            Nickname = logoutContext.NickName;
            Reason = logoutContext.Reason;
        }

        [SpParam(SqlParamName = "on_userid")]
        public int OnlineUserId { get; }

        public string UserName { get; }

        public byte RoleId { get; }

        public int ServerId { get; }

        public string Nickname { get; }

        public string Reason { get; }

        public override string GetSettingKey()
            => "Age_CheckOut";

        public override bool IsValid()
            => OnlineUserId > 0
            && !string.IsNullOrWhiteSpace(UserName)
            && RoleId >= 0
            && ServerId > 0;
    }
}
