namespace CN88.Auth.Criteria.Password
{
    public class ResetMainAccountPasswordCriteria : PasswordNonQueryCommand
    {
        public ResetMainAccountPasswordCriteria(
            int custId,
            int subAccId,
            string userPwd,
            bool disableCasino,
            string loginIP,
            int userId,
            string language) : base(custId, userPwd, loginIP, userId, language)
        {
            SubAccId = subAccId;
            DisCasino = disableCasino;
        }

        public int SubAccId { get;  }

        public bool DisCasino { get; }

        public override string GetSettingKey()
            => "ResetMainAccountPassword";
    }
}
