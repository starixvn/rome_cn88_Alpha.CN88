namespace Alpha.CN88.Tests.Models.Menus
{
    using System;
    using Alpha.CN88.Configurations;
    using Alpha.CN88.Models.Menus;
    using KellermanSoftware.CompareNetObjects;
    using NUnit.Framework;

    [TestFixture]
    public class MenuItemTests
    {
        private CompareLogic compareLogic;
        private Func<MenuStatus> getMenuStatus;
        private Predicate<string> authorizeUrlFunc;
        private Func<string> getDisplayName;
        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            getMenuStatus = () => new MenuStatus(true, false);
            authorizeUrlFunc = (preString) => false;
            getDisplayName = () => "Test";
        }

        [Test]
        public void Contructor_NotNullGetMenuStatusFunction_SetupCorrectMenuStatus()
        {
            // Arrange
            var menuItem = new MenuItem(getMenuStatus, null, null);
            var expectedValue = new MenuStatus(true, false);

            // Act
            var result = compareLogic.Compare(menuItem.Status, expectedValue).AreEqual;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Contructor_NullGetMenuStatusFunction_SetupCorrectMenuStatus()
        {
            // Arrange
            var menuItem = new MenuItem(null, null, null);
            var expectedValue = new MenuStatus(true, false);

            // Act
            var result = compareLogic.Compare(menuItem.Status, expectedValue).AreEqual;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Contructor_NotNullAuthorizeUrlFunction_SetupCorrectMenuStatus()
        {
            // Arrange
            var menuItem = new MenuItem(getMenuStatus, authorizeUrlFunc, null);
            menuItem.ByPassAuthorization = false;
            var expectedValue = new MenuStatus(false, false);

            // Act
            var result = compareLogic.Compare(menuItem.Status, expectedValue).AreEqual;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Contructor_NullAuthorizeUrlFunction_SetupCorrectMenuStatus()
        {
            // Arrange
            var menuItem = new MenuItem(getMenuStatus, null, null);
            menuItem.ByPassAuthorization = false;
            var expectedValue = new MenuStatus(true, false);

            // Act
            var result = compareLogic.Compare(menuItem.Status, expectedValue).AreEqual;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Contructor_NotNullGetDisplayName_DisplayCorrectName()
        {
            // Arrange
            var menuItem = new MenuItem(getMenuStatus, authorizeUrlFunc, getDisplayName);
            menuItem.ResourceKey = "home";
            var expectedValue = "Test";

            // Act
            var result = menuItem.DisplayName;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void Contructor_NullGetDisplayName_DisplayCorrectName()
        {
            // Arrange
            var menuItem = new MenuItem(getMenuStatus, authorizeUrlFunc, null);
            menuItem.ResourceKey = "home";
            var expectedValue = "Home";

            // Act
            var result = menuItem.DisplayName;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void Link_NullOrWhiteSpaceAppPath_ReturnLink()
        {
            // Arrange
            var menuItem = new MenuItem(getMenuStatus, authorizeUrlFunc, getDisplayName);
            menuItem.AppPath = "test";
            menuItem.Link = "temp";
            AppSettings.AppSettingValues["test"] = "";
            var expectedValue = "temp";

            // Act
            var result = menuItem.Link;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}
