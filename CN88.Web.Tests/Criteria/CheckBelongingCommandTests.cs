namespace CN88.Web.Tests.Criteria
{
    using CN88.Web.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class CheckBelongingCommandTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectValue()
        {
            // Arrange
            var command = new CheckBelongingCommand(1, 0, 2);

            // Act & Assert
            Assert.AreEqual("CS_CustomerSelCheckBelonging", command.GetSettingKey());
        }
    }
}
