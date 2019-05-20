namespace CN88.Auth.Criteria.NickName
{
    using Fanex.Data.Repository;

    public abstract class NickNameNonQueryCommand : NonQueryCommand
    {
        protected NickNameNonQueryCommand(
            int custId,
            string ip,
            string language)
        {
            CustId = custId;
            IP = ip;
            Language = language;
        }

        public int CustId { get; }

        public string IP { get; }

        public string Language { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
            => CustId > 0;
    }
}
