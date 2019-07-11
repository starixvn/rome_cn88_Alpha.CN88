namespace CN88.Auth.Params.Password
{
    using System.Collections.Generic;
    using Criteria.Password;

    public class ResetSubAccountPasswordParam : PasswordParamBase
    {
        public ResetSubAccountPasswordParam(
            int custId,
            int subAccId,
            string newPassword,
            string hashKey,
            string ip,
            int actorId,
            string languageKey,
            IEnumerable<string> invalidPasswords) : base(custId, newPassword, hashKey, ip, actorId, languageKey, invalidPasswords)
        {
            SubAccountId = subAccId;
        }

        public int SubAccountId { get; }

        public override PasswordNonQueryCommand ToCommand()
            => new ResetSubAccountPasswordCriteria(
                CustId,
                SubAccountId,
                NewPassword,
                IP,
                ActorId,
                LanguageKey);
    }
}