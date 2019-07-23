namespace Alpha.CN88.Tests.Services
{
    using System.Web;
    using Alpha.CN88.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SiteMapReaderTests
    {
        SiteMapProvider provider;
        SiteMapNode siteMapNode;

        [SetUp]
        public void SetUp()
        {
            provider = Substitute.For<SiteMapProvider>();
            siteMapNode = Substitute.For<SiteMapNode>(provider, "test");
        }

        [Test]
        public void GetRootNode_NotNullRootNode_ReturnRootNode()
        {
            // Arrange
            var reader = new SiteMapReaderImpl();

            // Act
            var result = reader.GetRootNode(siteMapNode);

            //Assert
            Assert.AreEqual(siteMapNode, result);
        }
    }
}
