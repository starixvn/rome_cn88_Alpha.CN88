namespace CN88.Web.Models
{
    public class CustomerStatus
    {
        private const byte Inactive = 1;
        private const byte Suspended = 2;

        public CustomerStatus()
        {
        }

        public CustomerStatus(bool closed, byte status, bool isSubAccount)
        {
            IsClosed = closed;
            CustStatus = status;
            IsSubAccount = isSubAccount;
        }

        public bool IsClosed { get; }

        public byte CustStatus { get; }

        public bool IsSubAccount { get; set; }

        public bool IsKickedOut(byte currentStatus)
            => IsClosed || CustStatus != currentStatus;

        public bool IsOK(byte currentStatus)
            => !IsKickedOut(currentStatus);

        public string GetErrorResourceKey()
        {
            if (IsClosed)
            {
                return IsSubAccount ? "SubAccountLockedError" : "CustomerLockedError";
            }

            if (CustStatus == Inactive)
            {
                return "CustomerInactiveError";
            }

            if (CustStatus == Suspended)
            {
                return "CustomerSuspendedError";
            }

            return "AccountStatusChangedError";
        }
    }
}
