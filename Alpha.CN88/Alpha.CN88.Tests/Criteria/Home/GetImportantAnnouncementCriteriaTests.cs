namespace Alpha.CN88.Tests.Criteria.Home
{
    using Alpha.CN88.Criteria.Home;
    using NUnit.Framework;

    [TestFixture]
    public class GetImportantAnnouncementCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetImportantAnnouncementCriteria(99, "ad");
            var expectedKey = "CS_AccountingPeriod_Sel";

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedKey, settingKey);
        }

        [Test]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetImportantAnnouncementCriteria(99, "ad");

            // Act
            var valid = criteria.IsValid();

            // Assert
            Assert.IsTrue(valid);
        }
    }
}

