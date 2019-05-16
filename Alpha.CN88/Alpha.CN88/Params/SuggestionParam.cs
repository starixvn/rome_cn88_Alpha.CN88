namespace Alpha.CN88.Params
{
    public class SuggestionParam
    {
        public SuggestionParam(
            int userId,
            int custId,
            bool isInternal,
            string username,
            bool isSubAccount,
            bool excludeMember,
            bool directDownline,
            bool isAccInfo
            )
        {
            UserId = userId;
            CustId = custId;
            IsInternal = isInternal;
            Username = username;
            IsSubAccount = isSubAccount;
            ExcludeMember = excludeMember;
            DirectDownline = directDownline;
            IsAccInfo = isAccInfo;
        }

        public int UserId { get; }

        public int CustId { get; }

        public bool IsInternal { get; }

        public string Username { get; }

        public bool IsSubAccount { get; }

        public bool ExcludeMember { get; }

        public bool DirectDownline { get; }

        public bool IsAccInfo { get; }
    }
}