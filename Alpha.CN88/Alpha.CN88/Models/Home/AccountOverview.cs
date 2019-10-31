namespace Alpha.CN88.Models.Home
{
    using System;
    using Alpha.CN88.Extensions;

    public class AccountOverview
    {
        public AccountOverview()
        {
        }

        public AccountOverview(
            decimal credit,
            decimal creditBalance,
            int newMembers,
            DateTime lastLogin,
            DateTime lastPasswordChangedDate)
        {
            Credit = credit;
            CreditBalance = creditBalance;
            NewMembers = newMembers;
            LastLogin = lastLogin;
            LastPasswordChangedDate = lastPasswordChangedDate;
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