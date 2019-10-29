namespace Alpha.CN88.Site.Tests.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.CN88.Models;
    using Alpha.CN88.Models.Home;
    using Alpha.CN88.Services;
    using Alpha.CN88.Site.Handlers;
    using Alpha.CN88.Site.Requests.Home;
    using Alpha.CN88.Site.ViewModels.Home;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HomeHandlerTests
    {
        private HomeService homeService;
        private HomeHandler handler;
        private IUser viewer;

        [SetUp]
        public void SetUp()
        {
            homeService = Substitute.For<HomeService>();
            handler = new HomeHandler(homeService);
            viewer = Substitute.For<IUser>();
            viewer.AccountManagementEditView.Returns(true);
        }

        [Test]
        public void HandleHomeQuery_Always_ReturnsHomeViewModel()
        {
            // arrange
            var message = new HomeQuery(viewer);
            var model = Tuple.Create<IEnumerable<AccountOverview>, IEnumerable<StatusSummary>>(
                new List<AccountOverview>() { new AccountOverview() },
                Enumerable.Empty<StatusSummary>());
            homeService.GetGeneralInformation(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<bool>())
               .Returns(model);
            var expectedResult = new HomeViewModel(message, model.Item1.FirstOrDefault(), model.Item2);

            // act
            var actualResult = handler.Handle(message);

            // assert
            Assert.IsTrue(new CompareLogic().Compare(expectedResult, actualResult).AreEqual);
        }
    }
}