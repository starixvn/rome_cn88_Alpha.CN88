namespace CN88.Web.Tests.Criteria
{
    using CN88.Web.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class ReCheckinCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectValue()
        {
            // Arrange
            var command = new ReCheckinCommand(1);

            // Act & Assert
            Assert.AreEqual("Age_ReCheckin", command.GetSettingKey());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidOnlineUserId_ReturnsFalse(int onlineUserId)
        {
            // Arrange
            var criteria = new ReCheckinCommand(onlineUserId);

            // Act & Assert
            Assert.IsFalse(criteria.IsValid());
        }

        [Test]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new ReCheckinCommand(1);

            // Act & Assert
            Assert.IsTrue(criteria.IsValid());
        }
    }
}
