namespace CN88.Auth.Criteria.Password
{
    public class UpdateSubAccountPasswordCriteria : PasswordNonQueryCommand
    {
        public UpdateSubAccountPasswordCriteria(
            int custId,
            int subAccId,
            string oldPwd,
            string userPwd,
            string loginIP,
            int userId,
            string language) : base(custId, userPwd, loginIP, userId, language)
        {
            SubAccId = subAccId;
            OldPwd = oldPwd;
        }

        public int SubAccId { get; }

        public string OldPwd { get; }

        public override string GetSettingKey()
            => "UpdateSubAccountPassword";

        public override bool IsValid()
            => base.IsValid()
            && SubAccId > 0
            && !string.IsNullOrWhiteSpace(OldPwd);
    }
}
