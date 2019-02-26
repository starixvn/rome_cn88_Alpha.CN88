namespace CN88.Auth.Params.Password
{
    using System.Collections.Generic;
    using Criteria.Password;

    public class ResetMainAccountPasswordParam : ResetSubAccountPasswordParam
    {
        public ResetMainAccountPasswordParam(
            int custId,
            int subAccId,
            string newPassword,
            string hashKey,
            bool isCasinoDisabled,
            bool isResetMemberPassword,
            string ip,
            int actorId,
            string languageKey,
            IEnumerable<string> invalidPasswords) : base(custId, subAccId, newPassword, hashKey, ip, actorId, languageKey, invalidPasswords)
        {
            IsCasinoDisabled = isCasinoDisabled;
            IsResetMemberPassword = isResetMemberPassword;
        }

        public bool IsCasinoDisabled { get; }

        public bool IsResetMemberPassword { get; }

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
