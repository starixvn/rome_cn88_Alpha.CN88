namespace Alpha.CN88.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Alpha.CN88.Models.Menus;
    using Alpha.CN88.Services.Impl;
    using global::CN88.Web.Services;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MenuItemBuilderTests
    {
        AuthorizationService service;
        IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        IDictionary<string, Func<string>> menuItemDisplayName;
        MenuItemBuilderImpl builder;
        CompareLogic compareLogic;


        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            service = Substitute.For<AuthorizationService>();
            service.IsAuthorized(Arg.Any<string>()).Returns(true);
            menuItemTypeConfiguration = Substitute.For<IDictionary<string, Func<MenuStatus>>>();
            menuItemTypeConfiguration[Arg.Any<string>()].Returns(() => new MenuStatus(true, false));
            menuItemDisplayName = new Dictionary<string, Func<string>>();
            menuItemDisplayName.Add(new KeyValuePair<string, Func<string>>("test", () => "temp"));

            builder = new MenuItemBuilderImpl(service, menuItemTypeConfiguration, menuItemDisplayName);
        }

        [Test]
        public void BuildMenuItem_SiteMapNodeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            TestDelegate testDelegate = () => builder.BuildMenuItem(null);

            // Assert
            Assert.Throws<ArgumentNullException>(testDelegate);
        }

        [Test]
        public void BuildMenuItem_SiteMapNodeNotNull_ReturnCorrectMenuItem()
        {
            // Arrange
            var provider = Substitute.For<SiteMapProvider>();
            var mapNode = Substitute.ForPartsOf<SiteMapNode>(provider, "temp");
            mapNode.ResourceKey = "test";
            mapNode.Url = "link";
            mapNode["AppPath"].Returns("AppPath");
            mapNode["isBeta"].Returns("false");
            mapNode["hideOnMobile"].Returns("true");
            mapNode["mobileResourceKey"].Returns("mobileResourceKey");
            mapNode["displayOnlyOnMobile"].Returns("true");
            mapNode["byPassAuthorization"].Returns("true");
            mapNode["menuItemType"].Returns("mapNode");
            var expectedMenu = new MenuItem(null, (preCon) => true, () => "temp");
            expectedMenu.AppPath = "AppPath";
            expectedMenu.IsBeta = false;
            expectedMenu.HideOnMobile = true;
            expectedMenu.MobileResourceKey = "mobileResourceKey";
            expectedMenu.DisplayOnlyOnMobile = true;
            expectedMenu.ByPassAuthorization = true;
            expectedMenu.SubMenus = new List<IMenuItem>();
            expectedMenu.Key = "temp";
            expectedMenu.ResourceKey = "test";
            expectedMenu.Link = "link";

            // Act
            var resultMenuItem = builder.BuildMenuItem(mapNode);
            var result = compareLogic.Compare(expectedMenu, resultMenuItem).AreEqual;

            // Assert
            Assert.IsTrue(result);
        }
    }
}
