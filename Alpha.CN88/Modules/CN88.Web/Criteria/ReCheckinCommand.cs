namespace CN88.Web.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    internal class ReCheckinCommand : NonQueryCommand
    {
        internal ReCheckinCommand(int onlineUserId)
        {
            OnlineUserId = onlineUserId;
        }

        [SpParam(SqlParamName = "on_userid")]
        public int OnlineUserId { get; }

        public bool IsKickedOut { get; set; }

        public override string GetSettingKey()
            => "Age_ReCheckin";

        public override bool IsValid()
            => OnlineUserId > 0;
    }
}
