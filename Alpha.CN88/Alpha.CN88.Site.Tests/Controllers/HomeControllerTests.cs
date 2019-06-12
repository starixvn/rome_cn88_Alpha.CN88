namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.CN88.Models.Home;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Requests.Home;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using Alpha.CN88.Site.Tests.TestModels;
    using Alpha.CN88.Site.ViewModels.Home;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HomeControllerTests : ControllerTestTemplate<HomeController>
    {
        IMediator mediator;

        protected override HomeController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();
            return new HomeController(mediator);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void Index_Always_ReturnHomeIndex()
        {
            // Arrange
            var expectedResult = "";

            // Act
            var result = Sut.Index() as ViewResult;

            // Arrange
            Assert.AreEqual(expectedResult, result.ViewName);
        }

        [Test]
        public void GeneralInformation_Always_ReturnParticalViewWithHomeViewModel()
        {
            // Arrange
            var statusSum = new List<StatusSummary>();
            var userTest = new UserTest();
            var homeQuery = new HomeQuery(userTest);
            var account = new AccountOverview();
            mediator.Send(Arg.Any<HomeQuery>()).Returns(new HomeViewModel(homeQuery, account, statusSum));

            // Act
            var result = Sut.GeneralInformation(homeQuery) as PartialViewResult;

            // Arrange
            Assert.IsInstanceOf<HomeViewModel>(result.Model);
        }
    }
}
