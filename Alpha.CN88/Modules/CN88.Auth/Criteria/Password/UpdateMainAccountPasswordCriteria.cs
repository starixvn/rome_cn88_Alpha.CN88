namespace CN88.Auth.Criteria.Password
{
    public class UpdateMainAccountPasswordCriteria : PasswordNonQueryCommand
    {
        public UpdateMainAccountPasswordCriteria(
            int custId,
            string oldPwd,
            string userPwd,
            bool disableCasino,
            string loginIP,
            int userId,
            string language) : base(custId, userPwd, loginIP, language)
        {
            OldPwd = oldPwd;
            DisCasino = disableCasino;
            UserId = userId;
        }
        public string OldPwd { get; }

        public bool DisCasino { get; }

        public int UserId { get; }

        public override string GetSettingKey()
            => "UpdateMainAccountPassword";

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(OldPwd)
            && UserId > 0;
    }
}
