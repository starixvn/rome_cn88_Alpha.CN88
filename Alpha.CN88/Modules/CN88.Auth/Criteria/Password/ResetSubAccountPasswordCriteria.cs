namespace CN88.Auth.Criteria.Password
{
    public class ResetSubAccountPasswordCriteria : PasswordNonQueryCommand
    {
        public ResetSubAccountPasswordCriteria(
            int custId,
            int subAccId,
            string userPwd,
            string loginIP,
            int userId,
            string language) : base(custId, userPwd, loginIP, language)
        {
            SubAccId = subAccId;
            UserId = userId;
        }

        public int SubAccId { get; }

        public int LoginSubAccId { get; }

        public int UserId { get; set; }

        public override string GetSettingKey()
            => "ResetSubAccountPassword";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}