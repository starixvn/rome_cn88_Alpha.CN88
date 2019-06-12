namespace Alpha.CN88.Tests.Models.Home
{
    using Alpha.CN88.Models.Home;
    using NUnit.Framework;

    [TestFixture]
    public class AccountOverviewTests
    {
        [Test]
        public void LastLoginDateStr_Always_ReturnCorrectValue()
        {
            // Arrage
            var overview = new AccountOverview();
            var expectedValue = "0001-01-01";
            // Act
            var result = overview.LastLoginDateStr;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}
