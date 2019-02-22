namespace Alpha.CN88.Site.ViewModels.Home
{
    using System;
    using CN88.Models;
    using Extensions;

    public class AccountOverviewViewModel
    {
        public AccountOverviewViewModel(AccountOverview model)
        {
            Credits = model.Credits;
            CreditBalance = model.CreditBalance;
            NewMembers = model.NewMembers;
            LastLoginDate = model.LastLoginDate;
            LastPasswordChangedDate = model.LastPasswordChangedDate;
        }

        public decimal Credits { get; }

        public decimal CreditBalance { get; }

        public int NewMembers { get; }

        public DateTime LastLoginDate { get; }

        public DateTime LastPasswordChangedDate { get; }

        public string LastLoginDateStr => LastLoginDate.ToShortDateFormat();

        public string LastPasswordChangedDateStr => LastPasswordChangedDate.ToShortDateFormat();
    }
}