namespace CN88.Auth.Params.NickName
{
    using Criteria.NickName;

    public class CreateNickNameParam
    {
        public CreateNickNameParam(
            int custId,
            string nickName,
            string ip,
            string languageKey)
        {
            CustId = custId;
            NickName = nickName;
            IP = ip;
            LanguageKey = languageKey;
        }

        public int CustId { get; }

        public string NickName { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public NickNameNonQueryCommand ToCommand()
            => new CreateNickNameCommand(CustId, NickName, IP, LanguageKey);
    }
}
