namespace Alpha.CN88.Tests.Models.Home
{
    using Alpha.CN88.Models.Home;
    using NUnit.Framework;

    [TestFixture]
    public class AccountingPeriodTests
    {
        [Test]
        public void IsCurrentPeriod_FromDateAndToDateIsToday_ReturnTrue()
        {
            // Arrange
            var period = new AccountingPeriod();

            // Act
            var result = period.IsCurrentPeriod;

            // Assert
            Assert.IsTrue(result);
        }
    }
}
