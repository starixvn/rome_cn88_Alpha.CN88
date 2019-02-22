namespace CN88.Auth.Criteria
{
    using Fanex.Data.Repository;

    public abstract class SCNonQueryCommand : NonQueryCommand
    {
        public int CustId { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
