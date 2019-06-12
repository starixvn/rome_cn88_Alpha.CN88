namespace Alpha.CN88.Site.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Requests;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using Alpha.CN88.Site.ViewModels;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class LanguageControllerTests : ControllerTestTemplate<LanguageController>
    {
        IMediator mediator;
        IUserSession userSession;

        protected override LanguageController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();

            return new LanguageController(mediator, userSession);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void Construcor_UserSessionNull_ThrowArrgumentNullException()
        {
            // Arrange
            TestDelegate testDelegate = () => new LanguageController(mediator, null);

            // Assert
            Assert.Throws<ArgumentNullException>(testDelegate);
        }

        [Test]
        public void List_Always_ReturnParticalViewWithLanguageViewModel()
        {
            // Arrange
            mediator.Send(Arg.Any<AllLanguagesQuery>()).Returns(new LanguageViewModel(new Dictionary<string, string>()));

            // Act
            var result = Sut.List() as PartialViewResult;

            // Arrange
            Assert.IsInstanceOf<LanguageViewModel>(result.Model);
        }

        [Test]
        public void Change_Always_ReturnParticalViewWithLanguageViewModel()
        {
            // Arrange
            mediator.Send(Arg.Any<ChangeLanguageCommand>()).Returns("language-code");
            var expetedResult = "language-code";
            // Act
            var result = Sut.Change("en-US");

            // Arrange
            Assert.AreEqual(expetedResult, result.Data);
        }
    }
}
