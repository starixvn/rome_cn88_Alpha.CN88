namespace Alpha.CN88.Models.Authentication
{
    public class AuthResult : Result
    {
        public AuthResult(int code, string message)
            : base(code, message)
        {
        }

        public AuthResult(
            int custId,
            string custName,
            int subAccId = 0,
            string subAccName = null,
            string deviceId = "N/A") : base(0, string.Empty)
        {
            CustId = custId;
            CustName = custName;
            SubAccId = subAccId;
            SubAccName = subAccName;
            DeviceId = deviceId;
        }

        public int CustId { get; }

        public string CustName { get; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public string DeviceId { get; }
    }
}
