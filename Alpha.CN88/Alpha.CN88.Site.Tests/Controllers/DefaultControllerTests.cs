namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Requests;
    using Alpha.CN88.Site.Requests.Default;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using Alpha.CN88.Site.Tests.TestModels;
    using global::CN88.Web.ViewModel;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultControllerTests : ControllerTestTemplate<DefaultController>
    {
        private IMediator mediator;
        private UserTest user;

        protected override DefaultController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();
            user = new UserTest();
            return new DefaultController(mediator, user);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void GetTotalOnlineMember_Always_ReturnJsonResult()
        {
            // Act
            var result = Sut.GetTotalOnlineMember();

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public void Index_Always_CallsMediatorSendBrowserQuery()
        {
            // Arrange
            var message = new IndexQuery(user);
            mediator.Send(Arg.Any<BrowserQuery>()).Returns(new BrowserViewModel());

            // Act
            Sut.Index(message);

            // Assert
            mediator.Received(1).Send(Arg.Any<BrowserQuery>());
        }

        [Test]
        public void Index_SupportBrowsers_CallsMediatorSendIndexQuery()
        {
            // Arrange
            var message = new IndexQuery(user);
            mediator.Send(Arg.Any<BrowserQuery>()).Returns(new BrowserViewModel { IsUnsupportedBrowser = false });

            // Act
            Sut.Index(message);

            // Assert
            mediator.Received().Send(message);
        }

        [Test]
        public void Index_UnSupportBrowsers_ReturnsUnSupportedBrowsersView()
        {
            // Arrange
            var message = new IndexQuery(user);
            mediator.Send(Arg.Any<BrowserQuery>()).Returns(new BrowserViewModel { IsUnsupportedBrowser = true });

            // Act
            var view = Sut.Index(message) as ViewResult;

            // Assert
            Assert.AreEqual("UnsupportedBrowsers", view.ViewName);
        }

        [Test]
        public void MenuBar_Always_CallsMediatorSendMenuBarQuery()
        {
            // Arrange
            var message = new MenuBarQuery(user.LevelId, RequestSub.UserAgent);

            // Act
            Sut.MenuBar();

            // Assert
            mediator.Received().Send(Arg.Is<MenuBarQuery>(m => new CompareLogic().Compare(m, message).AreEqual));
        }
    }
}