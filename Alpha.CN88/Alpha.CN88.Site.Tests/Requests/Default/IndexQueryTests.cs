namespace Alpha.CN88.Site.Tests.Requests.Default
{
    using Alpha.CN88.Constants;
    using Alpha.CN88.Site.Requests.Default;
    using Alpha.CN88.Site.Tests.TestModels;
    using NUnit.Framework;

    [TestFixture]
    public class IndexQueryTests
    {
        [TestCase(CustomerStatus.Active, true, true)]
        [TestCase(CustomerStatus.All, true, false)]
        [TestCase(CustomerStatus.All, false, false)]
        [TestCase(CustomerStatus.Active, false, false)]
        public void Contructor_Always_SetCorrectShowCN88SaveButton(byte customerStatus, bool accountManagementEditView, bool expectedValue)
        {
            // Arrange
            var user = new UserTest();
            user.CustomerStatus = customerStatus;
            user.AccountManagementEditView = accountManagementEditView;
            var query = new IndexQuery(user);

            // Assert
            Assert.AreEqual(expectedValue, query.ShowCN88SaveButton);
        }
    }
}
