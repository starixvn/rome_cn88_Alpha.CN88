namespace CN88.Auth.Models
{
    public class CustomerStatus
    {
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
    }
}
