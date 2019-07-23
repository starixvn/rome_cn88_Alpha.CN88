namespace Alpha.CN88.Tests.Models.Home
{
    using Alpha.CN88.Models.Home;
    using NUnit.Framework;

    [TestFixture]
    public class StatusSummaryTests
    {
        [Test]
        public void Add_StatusSummary_ReturnCorrectStatus()
        {
            // Arrange
            var summary = new StatusSummary();
            var extraSummary = new StatusSummary();
            var expectedValue = 0;

            // Act
            summary.Add(extraSummary);

            // Assert
            Assert.AreEqual(expectedValue, summary.Active);
        }
    }
}
