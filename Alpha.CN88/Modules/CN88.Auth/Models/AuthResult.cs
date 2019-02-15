namespace CN88.Auth.Models
{
    public class AuthResult
    {
        public AuthResult()
        {
        }

        public AuthResult(
            int custId,
            string custName,
            int subAccId = 0,
            string subAccName = null,
            string deviceId = "N/A")
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

        public bool IsSuccess
            => CustId > 0;
    }
}
