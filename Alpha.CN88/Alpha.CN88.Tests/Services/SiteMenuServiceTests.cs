namespace Alpha.CN88.Tests.Services
{
    using System.Collections.Generic;
    using System.Web;
    using Alpha.CN88.Models.Menus;
    using Alpha.CN88.Services;
    using Alpha.CN88.Services.Impl;
    using global::CN88.Web.Models;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SiteMenuServiceTests
    {
        private CompareLogic compareLogic;
        private CachingManager cachingManager;
        private SiteMapReader siteMapReader;
        private MenuItemBuilder menuItemBuilder;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();

            cachingManager = Substitute.For<CachingManager>();
            siteMapReader = Substitute.For<SiteMapReader>();
            menuItemBuilder = Substitute.For<MenuItemBuilder>();
        }

        [Test]
        public void GetMenuByUserLevel_NotNullCacheItemAndUsingCookie_ReturnCachedMenuItem()
        {
            // Arrange
            var expectedMenus = new List<IMenuItem>();
            cachingManager.Get<List<IMenuItem>>(Arg.Any<string>()).Returns(expectedMenus);
            var service = new SiteMenuServiceImpl(cachingManager, siteMapReader, menuItemBuilder);

            // Act
            var result = service.GetMenuByUserLevel(UserLevel.Headline, false);

            // Assert
            Assert.AreEqual(expectedMenus, result);

        }

        [Test]
        public void GetMenuByUserLevel_NullCacheItemAndUsingCookie_CacheMenuItem()
        {
            // Arrange
            var menuCache = "Alpha.CN88.MenuForLevel.Headline";
            var cacheMng = new CachingManagerImpl();
            cachingManager.Remove(menuCache);
            var siteMapProvider = Substitute.For<SiteMapProvider>();
            var siteRootNode = Substitute.ForPartsOf<SiteMapNode>(siteMapProvider, "tesst");
            var siteChildNode = Substitute.ForPartsOf<SiteMapNode>(siteMapProvider, "tesst");
            siteChildNode.Roles = new List<SiteMapNode>();
            var siteMapNodeCollections = new SiteMapNodeCollection();
            var childMapNodeCollections = Substitute.For<SiteMapNodeCollection>();
            childMapNodeCollections.Count.Returns(1);
            siteChildNode.ChildNodes = childMapNodeCollections;
            siteMapNodeCollections.Add(siteChildNode);
            siteRootNode.ChildNodes = siteMapNodeCollections;
            siteMapReader.GetRootNode().Returns(siteRootNode);
            var service = new SiteMenuServiceImpl(cacheMng, siteMapReader, menuItemBuilder);

            // Act
            service.GetMenuByUserLevel(UserLevel.Headline, false);

            // Assert
            Assert.IsNotNull(cacheMng.Get<List<IMenuItem>>(menuCache));

        }

        [Test]
        public void GetMenuByUserLevel_SiteMapNodeHasChild_AgentCookiless()
        {
            // Arrange
            var cacheMng = new CachingManagerImpl();
            var siteMapProvider = Substitute.For<SiteMapProvider>();
            var siteRootNode = Substitute.ForPartsOf<SiteMapNode>(siteMapProvider, "test");
            var siteChildNode = Substitute.ForPartsOf<SiteMapNode>(siteMapProvider, "test");
            siteChildNode.Roles = new List<SiteMapNode>();
            var siteMapNodeCollections = new SiteMapNodeCollection();
            var childMapNodeCollections = Substitute.For<SiteMapNodeCollection>();
            childMapNodeCollections.Count.Returns(1);
            siteChildNode.ChildNodes = childMapNodeCollections;
            siteMapNodeCollections.Add(siteChildNode);
            siteRootNode.ChildNodes = siteMapNodeCollections;
            siteMapReader.GetRootNode().Returns(siteRootNode);
            var expectedValue = new List<IMenuItem>();
            var menuItem = new MenuItem(null, (predict) => true, () => "test");
            expectedValue.Add(menuItem);
            menuItemBuilder.BuildMenuItem(Arg.Any<SiteMapNode>()).Returns(menuItem);
            var service = new SiteMenuServiceImpl(cacheMng, siteMapReader, menuItemBuilder);

            // Act
            var menuResult = service.GetMenuByUserLevel(UserLevel.Agent, true);
            var result = compareLogic.Compare(expectedValue, menuResult).AreEqual;

            // Assert
            Assert.IsTrue(result);
        }
    }
}
