namespace CN88.Auth.Params.Password
{
    using System.Collections.Generic;
    using Criteria.Password;

    public class ChangeMainAccountPasswordParam : ChangeSubAccountPasswordParam
    {
        public ChangeMainAccountPasswordParam(
            int custId,
            string currentPassword,
            string newPassword,
            string hashKey,
            bool isCasinoDisabled,
            string ip,
            int actorId,
            string languageKey,
            IEnumerable<string> invalidPasswords) : base(custId, 0, currentPassword, newPassword, hashKey, ip, actorId, languageKey, invalidPasswords)
        {
            IsCasinoDisabled = isCasinoDisabled;
        }

        public bool IsCasinoDisabled { get; }

        public override PasswordNonQueryCommand ToCommand()
            => new UpdateMainAccountPasswordCriteria(
                CustId,
                CurrentPassword,
                NewPassword,
                IsCasinoDisabled,
                IP,
                ActorId,
                LanguageKey);
    }
}
