namespace CN88.Web.Tests.Models
{
    using CN88.Web.Models;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class BelongingParamTests
    {
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(1);
            userSession.RoleId.Returns((byte)4);
            userSession.SubAccId.Returns(2);
        }

        [Test]
        public void Constructor_Always_AssignsCorrectValue()
        {
            // Arrange
            var param = new BelongingParam("/", userSession);

            // Act & Assert
            Assert.IsTrue("/" == param.CurrentURL
                && 1 == param.ActorId
                && 4 == param.ActorRoleId
                && !param.IsHeadline
                && 2 == param.ActorSubAccountId);
        }
    }
}
