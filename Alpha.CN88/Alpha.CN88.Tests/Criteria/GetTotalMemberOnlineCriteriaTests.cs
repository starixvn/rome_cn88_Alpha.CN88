namespace Alpha.CN88.Tests.Criteria
{
    using Alpha.CN88.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class GetTotalMemberOnlineCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetTotalMemberOnlineCriteria(1, 0, true);
            var expectedKey = "CS_Customer_TotalOnlineUser_Sel";

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
            var criteria = new GetTotalMemberOnlineCriteria(cusIdValue, 0, false);

            // Act
            var valid = criteria.IsValid();

            // Assert
            Assert.IsFalse(valid);
        }

        [Test]
        public void IsValid_CusIdPossitive_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetTotalMemberOnlineCriteria(99, 0, true);

            // Act
            var valid = criteria.IsValid();

            // Assert
            Assert.IsTrue(valid);
        }
    }
}