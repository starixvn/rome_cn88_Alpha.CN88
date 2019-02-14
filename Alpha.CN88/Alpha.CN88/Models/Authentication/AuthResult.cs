namespace Alpha.CN88.Models.Authentication
{
    public class AuthResult
    {
        public AuthResult()
        {
        }

        public AuthResult(int custId, int subAccId, string custName)
        {
            CustId = custId;
            SubAccId = subAccId;
            CustName = custName;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string CustName { get; }
    }
}
