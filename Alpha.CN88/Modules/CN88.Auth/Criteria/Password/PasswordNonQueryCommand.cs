namespace CN88.Auth.Criteria.Password
{
    using Fanex.Data.Repository;

    public abstract class PasswordNonQueryCommand : NonQueryCommand
    {
        public PasswordNonQueryCommand(
            int custId,
            string userPwd,
            string loginIP,
            int userId,
            string language)
        {
            CustId = custId;
            UserPwd = userPwd;
            LoginIP = loginIP;
            UserId = userId;
            Language = language;
        }

        public int CustId { get; }

        public string UserPwd { get; }

        public string LoginIP { get; }

        public int UserId { get; }

        public string Language { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
            => CustId > 0
            && !string.IsNullOrWhiteSpace(UserPwd)
            && UserId > 0;
    }
}
