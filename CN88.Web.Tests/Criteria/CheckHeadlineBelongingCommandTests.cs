namespace CN88.Web.Tests.Criteria
{
    using CN88.Web.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class CheckHeadlineBelongingCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectValue()
        {
            // Arrange
            var command = new CheckHeadlineBelongingCommand(1, 2);

            // Act & Assert
            Assert.AreEqual("CS_HeadlineSelCheckBelonging", command.GetSettingKey());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidSessionCustId_ReturnsFalse(int sessionCustId)
        {
            // Arrange
            var criteria = new CheckCustomerStatusCriteria(sessionCustId, 1);

            // Act & Assert
            Assert.IsFalse(criteria.IsValid());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new CheckHeadlineBelongingCommand(1, custId);

            // Act & Assert
            Assert.IsFalse(criteria.IsValid());
        }

        [Test]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new CheckHeadlineBelongingCommand(1, 1);

            // Act & Assert
            Assert.IsTrue(criteria.IsValid());
        }
    }
}
