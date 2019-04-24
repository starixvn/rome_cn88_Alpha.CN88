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
            string language) : base(custId, userPwd, loginIP, language)
        {
            SubAccId = subAccId;
            DisCasino = disableCasino;
            UserId = userId;
        }

        public int SubAccId { get; }

        public bool DisCasino { get; }

        public int UserId { get; }

        public override string GetSettingKey()
            => "ResetMainAccountPassword";

        public override bool IsValid()
            => base.IsValid() && UserId > 0;
    }
}
