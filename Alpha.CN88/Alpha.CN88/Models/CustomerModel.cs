namespace Alpha.CN88.Models
{
    using System;
    using Utilities.Extensions;

    public class CustomerModel
    {
        public CustomerModel()
        {
        }

        public CustomerModel(
            int custId,
            string username,
            string name,
            decimal credit,
            DateTime creationDate,
            int status,
            int closeReason)
        {
            CustId = custId;
            Username = username;
            Name = name;
            Credit = credit;
            CreationDate = creationDate;
            Status = status;
            CloseReason = closeReason;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Name { get; }

        public decimal Credit { get; }

        public DateTime CreationDate { get; }

        public string CreationDateStr => CreationDate.ToLongDateFormatWithSlash();

        public int Status { get; }

        public int CloseReason { get; }
    }
}
