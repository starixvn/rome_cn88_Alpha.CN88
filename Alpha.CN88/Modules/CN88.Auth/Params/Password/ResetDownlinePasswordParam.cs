namespace CN88.Auth.Params.Password
{
    using System.Collections.Generic;
    using Criteria.Password;

    public class ResetDownlinePasswordParam : ResetSubAccountPasswordParam
    {
        public ResetDownlinePasswordParam(
            int custId,
            int subAccId,
            string newPassword,
            string hashKey,
            bool isCasinoDisabled,
            string ip,
            int actorId,
            string languageKey,
            IEnumerable<string> invalidPasswords) : base(custId, subAccId, newPassword, hashKey, ip, actorId, languageKey, invalidPasswords)
        {
            IsCasinoDisabled = isCasinoDisabled;
        }

        public bool IsCasinoDisabled { get; }

        public override PasswordNonQueryCommand ToCommand()
            => new ResetMainAccountPasswordCriteria(
                CustId,
                SubAccountId,
                NewPassword,
                IsCasinoDisabled,
                IP,
                ActorId,
                LanguageKey);
    }
}
