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
            string languageKey,
            IEnumerable<string> invalidPasswords) : base(custId, newPassword, hashKey, ip, actorId, languageKey, invalidPasswords, currentPassword)
        {
            SubAccountId = subAccId;
        }

        public int SubAccountId { get; }

        public override PasswordNonQueryCommand ToCommand()
            => new UpdateSubAccountPasswordCriteria(
                CustId,
                SubAccountId,
                CurrentPassword,
                NewPassword,
                IP,
                LanguageKey);
    }
}
