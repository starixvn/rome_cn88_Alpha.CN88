namespace CN88.Auth.Params.NickName
{
    using Criteria.NickName;

    public class UpdateNickNameLoginOnlyParam
    {
        public UpdateNickNameLoginOnlyParam(
            int custId,
            bool nickNameLoginOnly,
            string ip,
            string languageKey)
        {
            CustId = custId;
            NickNameLoginOnly = nickNameLoginOnly;
            IP = ip;
            LanguageKey = languageKey;
        }

        public int CustId { get; }

        public bool NickNameLoginOnly { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public NickNameNonQueryCommand ToCommand()
            => new UpdateNickNameLoginOnlyCommand(CustId, NickNameLoginOnly, IP, LanguageKey);
    }
}
