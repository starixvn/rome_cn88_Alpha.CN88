namespace Alpha.CN88.Tests.Models.Menus
{
    using System;
    using Alpha.CN88.Models.Menus;
    using NUnit.Framework;

    [TestFixture]
    public class MenuStatusTests
    {
        private Func<bool> disableFunction;
        private Func<bool> enableFunction;

        [SetUp]
        public void SetUp()
        {
            disableFunction = () => true;

            enableFunction = () => true;
        }

        [Test]
        public void Contructor_ValidVisibleFunction_CorrectProperties()
        {
            // Arrange
            var menuStatus = new MenuStatus(enableFunction, disableFunction);

            // Act
            var result = menuStatus.Visible;

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Contructor_NullVisibleFunction_CorrectProperties()
        {
            // Arrange
            var menuStatus = new MenuStatus(null, disableFunction);

            // Act
            var result = menuStatus.Visible;

            //Assert
            Assert.IsTrue(result);
        }


        [Test]
        public void Contructor_ValidDisableFunction_CorrectProperties()
        {
            // Arrange
            var menuStatus = new MenuStatus(enableFunction, disableFunction);

            // Act
            var result = menuStatus.Disabled;

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Contructor_NullDisableFunction_CorrectProperties()
        {
            // Arrange
            var menuStatus = new MenuStatus(enableFunction, null);

            // Act
            var result = menuStatus.Disabled;

            //Assert
            Assert.IsFalse(result);
        }
    }
}
