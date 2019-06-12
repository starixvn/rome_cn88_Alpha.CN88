namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Models;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Requests.SecurityCode;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using Alpha.CN88.Site.Tests.TestModels;
    using global::CN88.Auth.Models;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SecurityCodeControllerTests : ControllerTestTemplate<SecurityCodeController>
    {
        IMediator mediator;
        UserTest userTest;
        protected override SecurityCodeController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();
            userTest = new UserTest();

            return new SecurityCodeController(mediator);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void ForceChangeFirstLogin_InValidResetSecurityCodeQuery_ReturnRedirectToRouteResult()
        {
            // Arrange
            var query = new ResetSecurityCodeQuery(userTest);

            // Act
            var result = Sut.ForceChangeFirstLogin(query);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void ForceChangeFirstLogin_ValidResetSecurityCodeQuery_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new ResetSecurityCodeQuery(userTest);

            // Act
            var result = Sut.ForceChangeFirstLogin(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ForceChangeFirstLogin_InValidResetSecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            var query = new ResetSecurityCodeCommand(userTest);

            // Act
            var result = Sut.ForceChangeFirstLogin(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ForceChangeFirstLogin_ValidResetSecurityCodeCommand_ReturnRedirectToRouteResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new ResetSecurityCodeCommand(userTest);
            mediator.Send(Arg.Any<ResetSecurityCodeCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.ForceChangeFirstLogin(query);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void ForceChangeSecurityCode_InValidChangeSecurityCodeQuery_ReturnRedirectToRouteResult()
        {
            // Arrange
            var query = new ChangeSecurityCodeQuery(userTest);

            // Act
            var result = Sut.ForceChangeSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void ForceChangeSecurityCode_ValidChangeSecurityCodeQuery_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            userTest.SecurityCode = "code";
            var query = new ChangeSecurityCodeQuery(userTest);

            // Act
            var result = Sut.ForceChangeSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ForceChangeSecurityCode_InValidChangeSecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            var query = new ChangeSecurityCodeCommand(userTest);

            // Act
            var result = Sut.ForceChangeSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ForceChangeSecurityCode_ValidChangeSecurityCodeCommand_ReturnRedirectToRouteResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            userTest.SecurityCode = "code";
            var query = new ChangeSecurityCodeCommand(userTest);
            mediator.Send(Arg.Any<ChangeSecurityCodeCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.ForceChangeSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void ChangeSecurityCode_AnyChangeSecurityCodeQuery_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new ChangeSecurityCodeQuery(userTest);

            // Act
            var result = Sut.ChangeSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ChangeSecurityCode_InValidChangeSecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            var query = new ChangeSecurityCodeCommand(userTest);

            // Act
            var result = Sut.ChangeSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ChangeSecurityCode_ValidChangeSecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            var query = new ChangeSecurityCodeCommand(userTest);
            mediator.Send(Arg.Any<ChangeSecurityCodeCommand>()).Returns(new SecurityResult(0, "Success"));

            // Acts
            var result = Sut.ChangeSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSecurityCode_AnyResetSecurityCodeQuery_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new ResetSecurityCodeQuery(userTest);

            // Act
            var result = Sut.ResetSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSecurityCode_InValidResetSecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            var query = new ResetSecurityCodeCommand(userTest);

            // Act
            var result = Sut.ResetSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSecurityCode_ValidResetSecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new ResetSecurityCodeCommand(userTest);
            mediator.Send(Arg.Any<ResetSecurityCodeCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.ResetSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSubAccountSecurityCode_ValidResetSecurityCodeQuery_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new ResetSecurityCodeQuery(userTest);

            // Act
            var result = Sut.ResetSubAccountSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSubAccountSecurityCode_InValidResetSecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            var query = new ResetSubAccountSecurityCodeCommand(userTest);

            // Act
            var result = Sut.ResetSubAccountSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSubAccountSecurityCode_ValidResetSecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new ResetSubAccountSecurityCodeCommand(userTest);
            mediator.Send(Arg.Any<ResetSubAccountSecurityCodeCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.ResetSubAccountSecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void VerifySecurityCode_AnyVerifySecurityCodeQuery_ReturnViewResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new VerifySecurityCodeQuery(userTest);

            // Act
            var result = Sut.VerifySecurityCode(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void VerifySecurityCode_InValidVerifySecurityCodeCommand_ReturnViewResult()
        {
            // Arrange
            var query = new VerifySecurityCodeCommand(userTest);
            var expectedResult = new Result(403, "You are not authorized on this function.");

            // Act
            var result = Sut.VerifySecurityCode(query) as JsonResult;
            var compareResult = compareLogic.Compare(expectedResult, result.Data).AreEqual;

            // Assert
            Assert.IsTrue(compareResult);
        }

        [Test]
        public void VerifySecurityCode_ValidVerifySecurityCodeCommand_ReturnRedirectToRouteResult()
        {
            // Arrange
            userTest.Language = "vi-VN";
            userTest.IsForcedSecurityCode = true;
            var query = new VerifySecurityCodeCommand(userTest);
            mediator.Send(Arg.Any<VerifySecurityCodeCommand>()).Returns(new SecurityResult(0, "Success"));
            var expectedResult = new SecurityResult(0, "Success");

            // Act
            var result = Sut.VerifySecurityCode(query) as JsonResult;
            var compareResult = compareLogic.Compare(expectedResult, result.Data).AreEqual;

            // Assert
            Assert.IsTrue(compareResult);
        }
    }
}
