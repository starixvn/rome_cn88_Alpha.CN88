namespace Alpha.CN88.Site.Tests.TestHelpers
{
    using System;
    using System.Collections;
    using System.Resources;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NSubstitute;

    public abstract class ControllerTestTemplate<TContext> : TestTemplate<TContext> where TContext : Controller
    {
        private const string RootPath = "/basepath";

        protected static Uri GetBaseUri() => new Uri("http://localhost:4000");

        protected HttpContextBase HttpContextSub { get; private set; }
        protected HttpRequestBase RequestSub { get; private set; }
        protected HttpResponseBase ResponseSub { get; private set; }

        public override void MainSetup()
        {
            base.MainSetup();
            Sut.ControllerContext = FakeControllerContext(Sut);
            Sut.Url = new UrlHelper(Sut.ControllerContext.RequestContext);
        }

        private ControllerContext FakeControllerContext(ControllerBase controller)
        {
            InitialiseFakeHttpContext();
            var routeData = new RouteData();
            routeData.Values.Add("key1", "value1");
            return new ControllerContext(HttpContextSub, routeData, controller);
        }

        private void InitialiseFakeHttpContext(string url = "")
        {
            HttpContextSub = Substitute.For<HttpContextBase>();
            RequestSub = Substitute.For<HttpRequestBase>();
            ResponseSub = Substitute.For<HttpResponseBase>();
            var serverUtilitySub = Substitute.For<HttpServerUtilityBase>();
            var itemsSub = Substitute.For<IDictionary>();
            HttpContextSub.Request.Returns(RequestSub);
            HttpContextSub.Response.Returns(ResponseSub);
            HttpContextSub.Server.Returns(serverUtilitySub);
            HttpContextSub.Items.Returns(itemsSub);
            serverUtilitySub.MapPath(Arg.Any<string>()).Returns("c:/absolute");
            RequestSub.ApplicationPath.Returns(RootPath);
            RequestSub.Url.Returns(GetBaseUri());
            RequestSub.Cookies.Returns(new HttpCookieCollection());
            if (!string.IsNullOrEmpty(url))
            {
                RequestSub.AppRelativeCurrentExecutionFilePath.Returns(url);
            }
            var resourceManagetSub = Substitute.For<ResourceManager>();
            resourceManagetSub.GetString("validMessage").Returns("Existed Message");
        }
    }
}
