namespace CN88.Auth.Params.Password
{
    using System.Collections.Generic;
    using Criteria.Password;

    public class ChangeSubAccountPasswordParam : PasswordParamBase
    {
        public ChangeSubAccountPasswordParam(
            int custId,
            int subAccId,
            string currentPassword,
            string newPassword,
            string hashKey,
            string ip,
            int actorId,
            string actorName,
            string languageKey,
            IEnumerable<string> invalidPasswords) : base(custId, newPassword, hashKey, ip, actorId, actorName, languageKey, invalidPasswords)
        {
            SubAccountId = subAccId;
            CurrentPassword = currentPassword;
        }

        public int SubAccountId { get; }

        public string CurrentPassword { get; set; }

        public override PasswordNonQueryCommand ToCommand()
            => new UpdateSubAccountPasswordCriteria(
                CustId,
                SubAccountId,
                CurrentPassword,
                NewPassword,
                IP,
                ActorId,
                LanguageKey);
    }
}
