namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Requests.NickName;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using Alpha.CN88.Site.Tests.TestModels;
    using Alpha.CN88.Site.ViewModels.NickName;
    using global::CN88.Auth.Models;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class NickNameControllerTests : ControllerTestTemplate<NickNameController>
    {
        IMediator mediator;

        protected override NickNameController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();
            return new NickNameController(mediator);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void Index_InValidCreateNickNameViewModel_ReturnRedirectToRouteResult()
        {
            // Arrange
            var userTest = new UserTest();
            var model = new CreateNickNameViewModel(userTest);
            // Act
            var result = Sut.Index(model);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }

        [Test]
        public void Index_ValidCreateNickNameViewModel_ReturnViewResult()
        {
            // Arrange
            var userTest = new UserTest();
            userTest.Language = "en-Us";
            userTest.NickName = "";
            userTest.IsInternal = false;
            var model = new CreateNickNameViewModel(userTest);
            // Act
            var result = Sut.Index(model);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Index_InValidCreateNickNameCommand_ReturnViewWWithSecurityResultForbidden()
        {
            // Arrange
            var userTest = new UserTest();
            var model = new CreateNickNameCommand(userTest);
            // Act
            var result = Sut.Index(model) as ViewResult;

            // Assert
            Assert.AreEqual(403, (result.Model as CreateNickNameViewModel).Result.Code);
        }

        [Test]
        public void Index_ValidCreateNickNameCommand_ReturnViewWWithCorrectCodeSecurityResult()
        {
            // Arrange
            var userTest = new UserTest();
            userTest.NickName = "";
            userTest.Language = "en-Us";
            userTest.IsInternal = false;
            var model = new CreateNickNameCommand(userTest);
            mediator.Send(Arg.Any<CreateNickNameCommand>()).Returns(new SecurityResult(200, "Success"));
            // Act
            var result = Sut.Index(model) as ViewResult;

            // Assert
            Assert.AreEqual(200, (result.Model as CreateNickNameViewModel).Result.Code);
        }

        [Test]
        public void Index_SecurityResultFail_ReturnRedirectToRouteResult()
        {
            // Arrange
            var userTest = new UserTest();
            userTest.NickName = "";
            userTest.Language = "en-Us";
            userTest.IsInternal = false;
            var model = new CreateNickNameCommand(userTest);
            mediator.Send(Arg.Any<CreateNickNameCommand>()).Returns(new SecurityResult(0, "Success"));

            // Act
            var result = Sut.Index(model);

            // Assert
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
        }
    }
}
