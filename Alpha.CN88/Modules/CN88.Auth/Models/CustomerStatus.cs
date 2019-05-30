namespace CN88.Auth.Models
{
    public class CustomerStatus
    {
        private const byte Inactive = 1;
        private const byte Suspended = 2;

        public CustomerStatus()
        {
        }

        public CustomerStatus(bool closed, byte status)
        {
            IsClosed = closed;
            CustStatus = status;
        }

        public bool IsClosed { get; }

        public byte CustStatus { get; }

        public bool IsKickedOut(byte currentStatus)
            => IsClosed || CustStatus != currentStatus;

        public bool IsOK(byte currentStatus)
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

            return "AccountStatusChangedMessage";
        }
    }
}
