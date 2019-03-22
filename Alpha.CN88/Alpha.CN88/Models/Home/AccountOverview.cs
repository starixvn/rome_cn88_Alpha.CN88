namespace Alpha.CN88.Models.Home
{
    using System;
    using Alpha.CN88.Extensions;

    public class AccountOverview
    {
        public AccountOverview()
        {
        }        

        public decimal Credit { get; }

        public decimal CreditBalance { get; }

        public int NewMembers { get; }

        public DateTime LastLogin { get; }

        public DateTime LastPasswordChangedDate { get; }

        public string LastLoginDateStr => LastLogin.ToShortDateFormat();

        public string LastPasswordChangedDateStr => LastPasswordChangedDate.ToShortDateFormat();
    }
}
