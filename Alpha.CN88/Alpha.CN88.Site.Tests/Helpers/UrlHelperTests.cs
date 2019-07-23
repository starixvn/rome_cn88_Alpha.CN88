namespace Alpha.CN88.Site.Tests.Helpers
{
    using System.Collections.Generic;
    using Alpha.CN88.Site.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class UrlHelperTests
    {
        [TestCase("link", null, "paramValue", "link")]
        [TestCase("link", "", "paramValue", "link")]
        [TestCase("link", " ", "paramValue", "link")]
        [TestCase("link", "  ", "paramValue", "link")]
        [TestCase("link&param=p", "param", "paramValue", "link&param=paramValue")]
        [TestCase("link", "param", "paramValue", "link&param=paramValue")]
        public void ReplaceOrAddQueryStringParam_InputType_ReturnLinkWithParams(string link, string paramName, object paramValue, string expectedResult)
        {
            // Act
            var result = UrlHelper.ReplaceOrAddQueryStringParam(link, paramName, paramValue);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void AddQueryStringParams_EmptyRouterValues_ReturnLink()
        {
            // Arrange
            var link = "defaultLink";
            var expectedValue = "defaultLink";

            // Act
            var result = UrlHelper.AddQueryStringParams(link, new Dictionary<string, object>());

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void AddQueryStringParams_RouterValues_ReturnLinkWithParams()
        {
            // Arrange
            var link = "defaultLink";
            var routerValues = new Dictionary<string, object>();
            routerValues.Add("param", "paramValue");
            var expectedValue = "defaultLink?param=paramValue";

            // Act
            var result = UrlHelper.AddQueryStringParams(link, routerValues);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }


        [Test]
        public void ReplaceOrAddQueryStringParams_EmptyRouterValues_ReturnLink()
        {
            // Arrange
            var link = "defaultLink";
            var expectedValue = "defaultLink";

            // Act
            var result = UrlHelper.ReplaceOrAddQueryStringParams(link, new Dictionary<string, object>());

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void ReplaceOrAddQueryStringParams_SingleRouterValues_ReturnLinkWithParams()
        {
            // Arrange
            var link = "defaultLink";
            var routerValues = new Dictionary<string, object>();
            routerValues.Add("param", "paramValue");
            var expectedValue = "defaultLink?param=paramValue";

            // Act
            var result = UrlHelper.ReplaceOrAddQueryStringParams(link, routerValues);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void ReplaceOrAddQueryStringParams_MultipleRouterValues_ReturnLinkWithParams()
        {
            // Arrange
            var link = "defaultLink?param2=paramValue2";
            var routerValues = new Dictionary<string, object>();
            routerValues.Add("param1", "paramValue1");
            var expectedValue = "defaultLink?param2=paramValue2&param1=paramValue1";

            // Act
            var result = UrlHelper.ReplaceOrAddQueryStringParams(link, routerValues);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }
    }
}
