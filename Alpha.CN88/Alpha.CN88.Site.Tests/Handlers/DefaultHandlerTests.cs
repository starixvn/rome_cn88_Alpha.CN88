namespace Alpha.CN88.Site.Tests.Handlers
{
    using Alpha.CN88.Services;
    using Alpha.CN88.Site.Handlers;
    using Alpha.Web.Helpers;
    using global::CN88.Web.Helpers;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultHandlerTests
    {
        SessionManager sessionManager;
        BrowserHelper browserHelper;
        DefaultService defaultService;

        DefaultHandler Sut;

        [SetUp]
        public void SetUp()
        {
            sessionManager = Substitute.For<SessionManager>();
            browserHelper = Substitute.For<BrowserHelper>();
            defaultService = Substitute.For<DefaultService>();

            Sut = new DefaultHandler(sessionManager, browserHelper, defaultService);
        }

    }
}
