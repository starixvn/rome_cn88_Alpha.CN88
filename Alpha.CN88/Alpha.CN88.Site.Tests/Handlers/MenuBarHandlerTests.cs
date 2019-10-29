using System.Linq;
using Alpha.CN88.Models.Menus;
using Alpha.CN88.Services;
using Alpha.CN88.Site.Handlers;
using Alpha.CN88.Site.Requests.Default;
using Alpha.CN88.Site.ViewModels.Default;
using CN88.Common.Models;
using CN88.Web.Helpers;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using NUnit.Framework;

namespace Alpha.CN88.Site.Tests.Handlers
{
    [TestFixture]
    public class MenuBarHandlerTests
    {
        private SiteMenuService siteMenuService;
        private BrowserHelper browserHelper;
        private MenuBarHandler handler;

        [SetUp]
        public void SetUp()
        {
            siteMenuService = Substitute.For<SiteMenuService>();
            browserHelper = Substitute.For<BrowserHelper>();
            handler = new MenuBarHandler(siteMenuService, browserHelper);
        }

        [Test]
        public void HandleMenuBarQuery_Always_ReturnsMenuBarViewModel()
        {
            var message = new MenuBarQuery(UserLevel.Agent.Value, "useragent");
            var menuItems = Enumerable.Empty<IMenuItem>();
            siteMenuService.GetMenuByUserLevel(Arg.Any<UserLevel>(), Arg.Any<bool>()).Returns(menuItems);
            browserHelper.IsMobileOrTablet().Returns(false);
            var expectedResult = new MenuBarViewModel(menuItems, false);

            var actualResult = handler.Handle(message);

            Assert.IsTrue(new CompareLogic().Compare(expectedResult, actualResult).AreEqual);
        }
    }
}