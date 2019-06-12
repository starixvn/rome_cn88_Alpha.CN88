namespace Alpha.CN88.Site.Tests.Controllers
{
    using System.Web.Mvc;
    using Alpha.CN88.Site.Controllers;
    using Alpha.CN88.Site.Tests.TestHelpers;
    using Alpha.CN88.Site.Tests.TestModels;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultControllerTests : ControllerTestTemplate<DefaultController>
    {
        IMediator mediator;
        UserTest user;

        protected override DefaultController EstablishContext()
        {
            mediator = Substitute.For<IMediator>();
            user = new UserTest();
            return new DefaultController(mediator, user);
        }

        protected override void TestCleanup()
        {
        }

        [Test]
        public void GetTotalOnlineMember_Always_ReturnJsonResult()
        {
            // Act
            var result = Sut.GetTotalOnlineMember();

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }
    }
}
