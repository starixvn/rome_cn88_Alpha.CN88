namespace Alpha.CN88.Models
{
    using System;

    public class AccountOverview
    {
        public AccountOverview()
        {
        }

        public AccountOverview(
            decimal credits,
            decimal creditBalance,
            int newMembers,
            DateTime lastLoginDate,
            DateTime lastPasswordChangedDate)
        {
            Credits = credits;
            CreditBalance = creditBalance;
            NewMembers = newMembers;
            LastLoginDate = lastLoginDate;
            LastPasswordChangedDate = lastPasswordChangedDate;
        }

        public decimal Credits { get; }

        public decimal CreditBalance { get; }

        public int NewMembers { get; }

        public DateTime LastLoginDate { get; }

        public DateTime LastPasswordChangedDate { get; }
    }
}
