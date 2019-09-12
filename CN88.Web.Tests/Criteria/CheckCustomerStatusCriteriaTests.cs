namespace CN88.Web.Tests.Criteria
{
    using CN88.Web.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class CheckCustomerStatusCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectValue()
        {
            // Arrange
            var criteria = new CheckCustomerStatusCriteria(1, 0);

            // Act & Assert
            Assert.AreEqual("CS_CustomerStatusSel", criteria.GetSettingKey());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new CheckCustomerStatusCriteria(custId, 0);

            // Act & Assert
            Assert.IsFalse(criteria.IsValid());
        }

        [TestCase(-1)]
        public void IsValid_InvalidSubAccId_ReturnsFalse(int subAccId)
        {
            // Arrange
            var criteria = new CheckCustomerStatusCriteria(1, subAccId);

            // Act & Assert
            Assert.IsFalse(criteria.IsValid());
        }

        [Test]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new CheckCustomerStatusCriteria(1, 0);

            // Act & Assert
            Assert.IsTrue(criteria.IsValid());
        }
    }
}
