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
            string language) : base(custId, userPwd, loginIP, userId, language)
        {
            OldPwd = oldPwd;
            DisCasino = disableCasino;
        }
        public string OldPwd { get; }

        public bool DisCasino { get; }

        public override string GetSettingKey()
            => "UpdateMainAccountPassword";

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(OldPwd);
    }
}
