namespace Alpha.CN88.Tests.Helpers
{
    using System;
    using Alpha.CN88.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class SystemTimeTests
    {

        [Test]
        public void Today_Always_ReturnToday()
        {
            // Arrange
            var expectedValue = DateTime.Today;
            // Act
            var result = SystemTime.Today;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void Today_CustomToday_ReturnCustomToday()
        {
            // Arrange
            var today = DateTime.Today;
            var expectedValue = today.AddDays(2);
            SystemTime.Today = today.AddDays(2);

            // Act
            var result = SystemTime.Today;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}
