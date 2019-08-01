namespace Alpha.CN88.Tests.Helpers
{
    using System;
    using Alpha.CN88.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class CookieHelperTests
    {
        private Func<string, string> encodeString;

        [SetUp]
        public void SetUp()
        {
            encodeString = (preString) => $"{preString}-{preString}";
        }

        [Test]
        public void GetCookieNameOfCurrentPage_Always_ReturnCorrectString()
        {
            // Arrange
            var expectedValue = "12_11_CN88-12_11_CN88";

            // Act
            var result = CookieHelper.GetCookieNameOfCurrentPage(12, 11, encodeString);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetCookieNameOfCurrentPage_NoFunctionEncript_ReturnUnencriptString()
        {
            // Arrange
            var expectedValue = "12_11_CN88";

            // Act
            var result = CookieHelper.GetCookieNameOfCurrentPage(12, 11, null);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}
