namespace Alpha.CN88.Site.Tests.Requests.Authentication
{
    using Alpha.CN88.Site.Requests.Authentication;
    using Alpha.CN88.Site.Tests.TestModels;
    using global::CN88.Auth.Constants;
    using NUnit.Framework;

    [TestFixture]
    public class LogoutQueryTests
    {
        [Test]
        public void Contructor_Always_SetReasonNormal()
        {
            // Arrange
            var user = new UserTest();
            var query = new LogoutQuery(user);
            var expectedValue = LogoutReason.Normal;

            // Assert
            Assert.AreEqual(expectedValue, query.Reason);
        }
    }
}
