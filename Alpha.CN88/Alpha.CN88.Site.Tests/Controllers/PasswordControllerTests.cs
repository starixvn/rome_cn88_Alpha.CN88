namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Requests.Password;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using Alpha.CN88.Site.Tests.TestModels;
    using global::CN88.Auth.Models;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PasswordControllerTests : ControllerTestTemplate<PasswordController>
    {
        IMediator mediator;

        protected override PasswordController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();

            return new PasswordController(mediator);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void ForceChangePassword_InVaidChangePasswordQuery_ReturnRedirectToRouteResult()
        {
            // Arrange
            var user = new UserTest();
            var query = new ChangePasswordQuery(user);

            // Act
            var result = Sut.ForceChangePassword(query);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void ForceChangePassword_VaidChangePasswordQuery_ReturnRedirectToRouteResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ChangePasswordQuery(user);

            // Act
            var result = Sut.ForceChangePassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ForceChangePassword_InVaidChangePasswordCommand_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            var query = new ChangePasswordCommand(user);

            // Act
            var result = Sut.ForceChangePassword(query) as ViewResult;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ForceChangePassword_VaidChangePasswordCommand_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ChangePasswordCommand(user);
            mediator.Send(Arg.Any<ChangePasswordCommand>()).Returns(new SecurityResult(200, "Success"));

            // Act
            var result = Sut.ForceChangePassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ForceChangePassword_VaidChangePasswordCommand_ReturnRedirectToRouteResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ChangePasswordCommand(user);
            mediator.Send(Arg.Any<ChangePasswordCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.ForceChangePassword(query);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void ChangePassword_WithChangePasswordQuery_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ChangePasswordQuery(user);

            // Act
            var result = Sut.ChangePassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ChangePassword_InValidChangePasswordCommand_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            var query = new ChangePasswordCommand(user);

            // Act
            var result = Sut.ChangePassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ChangePassword_VaidChangePasswordCommand_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ChangePasswordCommand(user);
            mediator.Send(Arg.Any<ChangePasswordCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.ChangePassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetDownlinePassword_WithResetPasswordQuery_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ResetPasswordQuery(user);

            // Act
            var result = Sut.ResetDownlinePassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetDownlinePassword_InValidResetDownlinePasswordCommand_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            var query = new ResetDownlinePasswordCommand(user);

            // Act
            var result = Sut.ResetDownlinePassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetDownlinePassword_VaidResetDownlinePasswordCommand_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ResetDownlinePasswordCommand(user);
            mediator.Send(Arg.Any<ChangePasswordCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.ResetDownlinePassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSubAccountPassword_WithResetPasswordQuery_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ResetPasswordQuery(user);

            // Act
            var result = Sut.ResetSubAccountPassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSubAccountPassword_InValidResetSubAccountPasswordCommand_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            var query = new ResetSubAccountPasswordCommand(user);

            // Act
            var result = Sut.ResetSubAccountPassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void ResetSubAccountPassword_VaidResetSubAccountPasswordCommand_ReturnViewResult()
        {
            // Arrange
            var user = new UserTest();
            user.Language = "vi-VN";
            var query = new ResetSubAccountPasswordCommand(user);
            mediator.Send(Arg.Any<ChangePasswordCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.ResetSubAccountPassword(query);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
