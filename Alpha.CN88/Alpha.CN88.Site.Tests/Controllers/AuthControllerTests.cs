namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Requests.Authentication;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using global::CN88.Auth.Models;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AuthControllerTests : ControllerTestTemplate<AuthController>
    {
        IMediator mediator;

        protected override AuthController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();
            return new AuthController(mediator);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void ProcessLogin_AdminMessageInvalid_ReturnEmptyResult()
        {
            // Arrange
            var message = new AdminLoginQuery { Params = "", IBCKeyPath = "" };
            var expetedResult = new EmptyResult();

            // Act
            var loginResult = Sut.Login(message);
            var result = compareLogic.Compare(expetedResult, loginResult).AreEqual;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ProcessLogin_InvalidMessage_ReturnRedirectToRouteResult()
        {
            // Arrange
            var message = new LoginQuery { Params = "", IBCKeyPath = "" };

            // Act
            var loginResult = Sut.Login(message);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(loginResult);
        }

        [Test]
        public void ProcessLogin_AdminAuthFail_ReturnEmptyResult()
        {
            // Arrange
            var message = new AdminLoginQuery { Params = "test", IBCKeyPath = "index" };
            var authResult = new AuthResult(-1, "");
            mediator.Send(Arg.Any<AdminLoginQuery>()).Returns(authResult);
            var expetedResult = new EmptyResult();

            // Act
            var loginResult = Sut.Login(message);
            var result = compareLogic.Compare(expetedResult, loginResult).AreEqual;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ProcessLogin_AdminAuthFail_ReturnRedirectToRouteResult()
        {
            // Arrange
            var message = new LoginQuery { Params = "test", IBCKeyPath = "index" };
            var authResult = new AuthResult(-1, "");
            mediator.Send(Arg.Any<LoginQuery>()).Returns(authResult);

            // Act
            var loginResult = Sut.Login(message);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(loginResult);
        }

        [TestCase(null, "en-US", "You are not authorized on this function.")]
        [TestCase("", "en-US", "You are not authorized on this function.")]
        [TestCase(" ", "en-US", "You are not authorized on this function.")]
        [TestCase("  ", "en-US", "You are not authorized on this function.")]
        [TestCase("CustomerClosed", null, "Your account was closed so you're forced to logout. Please contact your upline for the assistance.")]
        [TestCase("CustomerClosed", "", "Your account was closed so you're forced to logout. Please contact your upline for the assistance.")]
        [TestCase("CustomerClosed", " ", "Your account was closed so you're forced to logout. Please contact your upline for the assistance.")]
        [TestCase("CustomerClosed", "  ", "Your account was closed so you're forced to logout. Please contact your upline for the assistance.")]
        [TestCase("CustomerClosed", "en-US", "Your account was closed so you're forced to logout. Please contact your upline for the assistance.")]
        public void Kickout_IsNullOrWhiteSpace_Message(string message, string cultureName, string expectedValue)
        {
            // Arrange

            // Act
            ViewResult result = Sut.Kickout(message, cultureName) as ViewResult;

            // Assert
            Assert.AreEqual(expectedValue, result.Model);
        }
    }
}
