namespace Alpha.CN88.Tests.Models.Home
{
    using System;
    using Alpha.CN88.Models.Home;
    using NUnit.Framework;

    [TestFixture]
    public class AccountOverviewTests
    {
        [Test]
        public void Constructor_Always_ReturnCorrectValue()
        {
            // Arrage
            var lastLoginDate = new DateTime(2019, 10, 31);
            var lastChangePwdDate = new DateTime(2019, 10, 30);

            // Act
            var overview = new AccountOverview(10m, 5m, 2, lastLoginDate, lastChangePwdDate);

            // Assert
            Assert.IsTrue(
                overview.Credit == 10m
                && overview.CreditBalance == 5m
                && overview.NewMembers == 2
                && overview.LastLogin == lastLoginDate
                && overview.LastPasswordChangedDate == lastChangePwdDate
                && overview.LastLoginDateStr == "2019-10-31"
                && overview.LastPasswordChangedDateStr == "2019-10-30");
        }
    }
}