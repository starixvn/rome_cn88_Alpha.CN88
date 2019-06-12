namespace Alpha.CN88.Site.Tests.Requests.Home
{
    using Alpha.CN88.Site.Requests.Home;
    using Alpha.CN88.Site.Tests.TestModels;
    using NUnit.Framework;

    [TestFixture]
    public class HomeQueryTests
    {
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        [TestCase(true, true, true)]
        [TestCase(true, false, true)]
        public void Contructor_Always_SetCorrectHasAccountManagement(bool accountManagementViewOnly, bool accountManagementEditView, bool expectedResult)
        {
            // Arrange
            var user = new UserTest();
            user.AccountManagementViewOnly = accountManagementViewOnly;
            user.AccountManagementEditView = accountManagementEditView;
            var query = new HomeQuery(user);

            // Assert
            Assert.AreEqual(expectedResult, query.HasAccountManagement);
        }
    }
}
