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

        public int LoginSubAccId { get; }

        public override string GetSettingKey()
            => "ResetSubAccountPassword";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
