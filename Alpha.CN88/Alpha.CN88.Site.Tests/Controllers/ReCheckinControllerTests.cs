namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Models;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using Alpha.CN88.Site.Tests.TestModels;
    using global::CN88.Auth.Models;
    using global::CN88.Auth.Services;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ReCheckinControllerTests : ControllerTestTemplate<ReCheckinController>
    {
        IUser user;
        AuthenticationService authenticationService;

        protected override ReCheckinController EstablishContext()
        {
            user = new UserTest();
            authenticationService = Substitute.For<AuthenticationService>();
            return new ReCheckinController(user, authenticationService);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void Index_IsDisabledCheckDuplicationLogin_ReturDefaultSecurityResult()
        {
            // Arrange
            authenticationService.IsDisabledCheckDuplicationLogin.Returns(true);
            var expectedResult = new SecurityResult();

            // Act
            var result = Sut.Index(12) as JsonResult;
            var compairResult = compareLogic.Compare(expectedResult, result.Data).AreEqual;

            // Assert
            Assert.IsTrue(compairResult);
        }
    }
}
