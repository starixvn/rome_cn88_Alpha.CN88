namespace CN88.Auth.Models
{
    public class CustomerStatus
    {
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
    }
}
