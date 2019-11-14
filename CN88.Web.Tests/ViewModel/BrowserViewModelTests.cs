namespace CN88.Web.Tests.ViewModel
{
    using CN88.Common.Models;
    using CN88.Web.ViewModel;
    using NUnit.Framework;

    [TestFixture]
    public class BrowserViewModelTests
    {
        [Test]
        public void Properties_Always_AssignsCorrectValue()
        {
            // Arrange
            var viewModel = new BrowserViewModel
            {
                IsUnsupportedBrowser = false,
                DownloadNewBrowserLink = "http://abc//xyz",
                UserLevel = UserLevel.Agent
            };

            // Act & Assert
            Assert.IsTrue(!viewModel.IsUnsupportedBrowser
                && "http://abc//xyz" == viewModel.DownloadNewBrowserLink
                && viewModel.UserLevel.IsAgentLevel());
        }
    }
}
