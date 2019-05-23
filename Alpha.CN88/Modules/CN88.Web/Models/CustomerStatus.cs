namespace CN88.Web.Models
{
    public class CustomerStatus
    {
        private const int Inactive = 1;
        private const int Suspended = 2;

        public CustomerStatus()
        {
        }

        public CustomerStatus(bool closed, int status)
        {
            IsClosed = closed;
            CustStatus = status;
        }

        public bool IsClosed { get; }

        public int CustStatus { get; }

        public bool IsKickedOut(int currentStatus)
            => IsClosed || CustStatus != currentStatus;

        public bool IsOK(int currentStatus)
            => !IsKickedOut(currentStatus);

        public string GetErrorResourceKey()
        {
            if (IsClosed)
            {
                return "CustomerLocked";
            }

            if (CustStatus == Inactive)
            {
                return "CustomerInactive";
            }

            if (CustStatus == Suspended)
            {
                return "CustomerSuspended";
            }

            return "SessionKickedOut";
        }
    }
}
