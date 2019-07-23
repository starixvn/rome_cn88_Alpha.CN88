namespace Alpha.CN88.Site.Tests.Handlers
{
    using Alpha.CN88.Services;
    using Alpha.CN88.Site.Handlers;
    using Alpha.CN88.Site.Requests;
    using Alpha.CN88.Site.Requests.Default;
    using Alpha.CN88.Site.Tests.TestModels;
    using Alpha.CN88.Site.ViewModels.Default;
    using Alpha.Web.Helpers;
    using global::CN88.Web.Helpers;
    using global::CN88.Web.ViewModel;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Utilities;

    [TestFixture]
    public class DefaultHandlerTests
    {
        SessionManager sessionManager;
        BrowserHelper browserHelper;
        DefaultService defaultService;

        DefaultHandler Sut;
        CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            sessionManager = Substitute.For<SessionManager>();
            browserHelper = Substitute.For<BrowserHelper>();
            defaultService = Substitute.For<DefaultService>();

            Sut = new DefaultHandler(sessionManager, browserHelper, defaultService);
            compareLogic = new CompareLogic();
        }

        [Test]
        public void HandlerBrowserQuery_SawBrowserWarningPageSessionKeyIsTrue_ReturnNewBrowserViewModel()
        {
            // Arrange
            sessionManager.Get<bool>(Arg.Any<string>()).Returns(true);
            var user = new UserTest();
            var query = new BrowserQuery(user);
            var expectedResult = new BrowserViewModel();

            // Act
            var result = Sut.Handle(query);
            var compareResult = compareLogic.Compare(expectedResult, result).AreEqual;

            // Assert
            Assert.IsTrue(compareResult);
        }

        [Test]
        public void HandlerIndexQuery_Always_ReturnIndexViewModel()
        {
            // Arrange
            defaultService.GetOnlineMember(Arg.Any<int>(), Arg.Any<int>(), false).Returns(5);
            var user = new UserTest();
            var query = new IndexQuery(user);
            var expectedResult = new IndexViewModel(query, browserHelper, 5, FunctionRegistry.GetDefaultUrl, Encryption.PearsonEncode);

            // Act
            var result = Sut.Handle(query);
            var compareResult = compareLogic.Compare(expectedResult, result).AreEqual;

            // Assert
            Assert.IsTrue(compareResult);
        }

        [Test]
        public void HandlerTotalOnlineMember_Always_ReturnCorrectValue()
        {
            // Arrange
            defaultService.GetOnlineMember(Arg.Any<int>(), Arg.Any<int>(), false).Returns(99);
            var user = new UserTest();
            var query = new TotalOnlineMember(user);
            var expectedResult = 99;

            // Act
            var result = Sut.Handle(query);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
