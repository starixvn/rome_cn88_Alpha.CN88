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
            string deviceId = "N/A",
            string afterLogInSite = "",
            string afterLogInQuery = "")
        {
            CustId = custId;
            CustName = custName;

            AfterLogInSite = afterLogInSite;
            AfterLogInQuery = afterLogInQuery;

            SubAccId = subAccId;
            SubAccName = subAccName;
            DeviceId = deviceId;
        }

        public int CustId { get; }

        public string CustName { get; }

        public int SubAccId { get; }

        public string SubAccName { get; }

        public string DeviceId { get; }

        public string AfterLogInSite { get; }

        public string AfterLogInQuery { get; }

        public bool IsSuccess
            => CustId > 0;
    }
}
