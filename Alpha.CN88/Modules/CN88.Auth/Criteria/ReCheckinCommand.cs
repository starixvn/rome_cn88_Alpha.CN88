namespace CN88.Auth.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ReCheckinCommand : NonQueryCommand
    {
        public ReCheckinCommand(int onlineUserId)
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
