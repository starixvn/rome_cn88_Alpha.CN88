namespace Alpha.CN88.Site.Tests.Requests.Authentication
{
    using Alpha.CN88.Site.Requests.Authentication;
    using NUnit.Framework;

    [TestFixture]
    public class LoginQueryTests
    {
        [TestCase(null, true)]
        [TestCase(" ", true)]
        [TestCase("  ", true)]
        [TestCase("", true)]
        [TestCase("validParam", false)]
        public void IsValid_AllParamCase_ReturnCorrectResult(string param, bool expectedResult)
        {
            // Arrange
            var loginQuery = new LoginQuery { Params = param, IBCKeyPath = "IBCKey" };

            // Assert
            Assert.AreEqual(expectedResult, loginQuery.IsInvalid());
        }
    }
}
