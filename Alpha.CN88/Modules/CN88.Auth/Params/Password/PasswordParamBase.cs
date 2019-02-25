namespace CN88.Auth.Params.Password
{
    using System.Collections.Generic;
    using Criteria.Password;

    public abstract class PasswordParamBase
    {
        public PasswordParamBase(
            int custId,
            string newPassword,
            string hashKey,
            string ip,
            int actorId,
            string actorName,
            string languageKey,
            IEnumerable<string> invalidPasswords)
        {
            CustId = custId;
            NewPassword = newPassword;
            HashKey = hashKey;
            IP = ip;
            ActorId = actorId;
            ActorName = actorName;
            LanguageKey = languageKey;
            InvalidPasswords = invalidPasswords;
        }

        public int CustId { get; }

        public string NewPassword { get; set; }

        public string HashKey { get; }

        public string IP { get; }

        public int ActorId { get; }

        public string ActorName { get; }

        public string LanguageKey { get; }

        public IEnumerable<string> InvalidPasswords { get; }

        public abstract PasswordNonQueryCommand ToCommand();
    }
}
