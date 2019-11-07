namespace CN88.Web.Tests.Models
{
    using CN88.Web.Models;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerStatusTests
    {
        [Test]
        public void Constructor_Default_AssignsCorrectValue()
        {
            // Arrange
            var param = new CustomerStatus();

            // Act & Assert
            Assert.IsTrue(!param.IsClosed
                && 0 == param.CustStatus
                && !param.IsSubAccount
                && "AccountStatusChangedError" == param.GetErrorResourceKey());
        }

        [Test]
        public void Constructor_Always_AssignsCorrectValue()
        {
            // Arrange
            var param = new CustomerStatus(true, 4, true);

            // Act & Assert
            Assert.IsTrue(param.IsClosed
                && 4 == param.CustStatus
                && param.IsSubAccount
                && "SubAccountLockedError" == param.GetErrorResourceKey());
        }
    }
}
