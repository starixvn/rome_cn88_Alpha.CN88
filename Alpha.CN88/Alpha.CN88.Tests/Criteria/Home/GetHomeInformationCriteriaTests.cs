namespace Alpha.CN88.Tests.Criteria.Home
{
    using System;
    using Alpha.CN88.Criteria.Home;
    using NUnit.Framework;

    [TestFixture]
    public class GetHomeInformationCriteriaTests
    {
        private DateTime yesterday;
        private DateTime today;
        private DateTime tomorrow;
        [SetUp]
        public void SetUp()
        {
            today = DateTime.Today;
            yesterday = today.AddDays(-1);
            tomorrow = today.AddDays(1);
        }
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetHomeInformationCriteria(99, yesterday, today);
            var expectedKey = "CS_Customer_Homepage_Sel";

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedKey, settingKey);
        }

        [TestCase(0)]
        [TestCase(-2)]
        public void IsValid_CusIdNotPossitive_ReturnsFalse(int cusIdValue)
        {
            // Arrange
            var criteria = new GetHomeInformationCriteria(cusIdValue, yesterday, today);

            // Act
            var valid = criteria.IsValid();

            // Assert
            Assert.IsFalse(valid);
        }

        [Test]
        public void IsValid_FromDateLargerToDate_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetHomeInformationCriteria(99, tomorrow, today);

            // Act
            var valid = criteria.IsValid();

            // Assert
            Assert.IsFalse(valid);
        }

        [Test]
        public void IsValid_InValidFromDate_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetHomeInformationCriteria(99, DateTime.MinValue, today);

            // Act
            var valid = criteria.IsValid();

            // Assert
            Assert.IsFalse(valid);
        }

        [Test]
        public void IsValid_InValidToDate_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetHomeInformationCriteria(99, today, DateTime.MaxValue);

            // Act
            var valid = criteria.IsValid();

            // Assert
            Assert.IsFalse(valid);
        }


        [Test]
        public void IsValid_ValidDateAndCusId_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetHomeInformationCriteria(99, yesterday, today);

            // Act
            var valid = criteria.IsValid();

            // Assert
            Assert.IsTrue(valid);
        }
    }
}

