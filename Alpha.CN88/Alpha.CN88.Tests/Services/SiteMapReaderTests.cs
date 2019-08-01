namespace Alpha.CN88.Tests.Services
{
    using System.Web;
    using Alpha.CN88.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SiteMapReaderTests
    {
        private SiteMapProvider provider;
        private SiteMapNode siteMapNode;
        private SiteMapReaderImpl siteMapReader;

        [SetUp]
        public void SetUp()
        {
            provider = Substitute.For<SiteMapProvider>();
            siteMapNode = Substitute.For<SiteMapNode>(provider, "test");
            siteMapReader = new SiteMapReaderImpl();
        }

        [Test]
        public void GetRootNode_NotNullRootNode_ReturnRootNode()
        {
            // Act
            var result = siteMapReader.GetRootNode(siteMapNode);

            //Assert
            Assert.AreEqual(siteMapNode, result);
        }
    }
}
