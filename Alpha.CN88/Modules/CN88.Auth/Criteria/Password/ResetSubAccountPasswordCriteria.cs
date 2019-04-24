namespace CN88.Auth.Criteria.Password
{
    public class ResetSubAccountPasswordCriteria : PasswordNonQueryCommand
    {
        public ResetSubAccountPasswordCriteria(
            int custId,
            int subAccId,
            string userPwd,
            string loginIP,
            string language) : base(custId, userPwd, loginIP, language)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }

        // Hard-code because store procedured does nothing with this param.
        public int LoginSubAccId { get; } = 0;

        public override string GetSettingKey()
            => "ResetSubAccountPassword";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
