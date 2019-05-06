namespace CN88.Auth.Params.NickName
{
    public class CreateNickNameParam
    {
        public CreateNickNameParam(
            int custId,
            int subAccountId,
            string nickName,
            string ip,
            string languageKey)
        {
            CustId = custId;
            SubAccountId = subAccountId;
            NickName = nickName;
            IP = ip;
            LanguageKey = languageKey;
        }

        public int CustId { get; }

        public int SubAccountId { get; }

        public string NickName { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public bool IsSubAccount => SubAccountId != 0;
    }
}
