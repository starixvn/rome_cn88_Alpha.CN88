namespace Alpha.CN88.Tests.Extensions
{
    using System;
    using Alpha.CN88.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void ToShortDateFormat_Always_ReturnFormatedDate()
        {
            // Arrange
            var expectedValue = "2019-07-15";
            var testDate = new DateTime(2019, 7, 15);

            // Act
            var result = testDate.ToShortDateFormat();

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}
