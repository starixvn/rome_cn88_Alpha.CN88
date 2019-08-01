namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class HelpControllerTests : ControllerTestTemplate<HelpController>
    {
        IMediator mediator;
        IUserSession userSession;
        protected override HelpController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            userSession.Language = "vi-VN";
            return new HelpController(mediator, userSession);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void NewUrl_Always_ReturnView()
        {
            // Act
            var result = Sut.NewUrl();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Troubleshooting_Always_ReturnTroubleshootingIndexView()
        {
            // Arrange
            var expectedResult = "~/Views/Help/Troubleshooting/vi-vn/Index.cshtml";
            // Act
            var result = Sut.Troubleshooting() as ViewResult;

            // Assert
            Assert.AreEqual(expectedResult, result.ViewName);
        }

        [Test]
        public void Cache_Always_ReturnTroubleshootingCacheView()
        {
            // Arrange
            var expectedResult = "~/Views/Help/Troubleshooting/vi-vn/Cache.cshtml";
            // Act
            var result = Sut.Cache() as ViewResult;

            // Assert
            Assert.AreEqual(expectedResult, result.ViewName);
        }

        [Test]
        public void LatestBrowser_Always_ReturnTroubleshootingLatestBrowserView()
        {
            // Arrange
            var expectedResult = "~/Views/Help/Troubleshooting/vi-vn/LatestBrowser.cshtml";
            // Act
            var result = Sut.LatestBrowser() as ViewResult;

            // Assert
            Assert.AreEqual(expectedResult, result.ViewName);
        }
    }
}
