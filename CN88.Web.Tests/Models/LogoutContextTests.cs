namespace CN88.Web.Tests.Models
{
    using CN88.Web.Models;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class LogoutContextTests
    {
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
        }

        [Test]
        public void Constructor_Always_AssignsCorrectValue()
        {
            // Arrange
            userSession.OnUserId.Returns(1);
            userSession.IsSubAcc.Returns(false);
            userSession.UserName.Returns("master1");
            userSession.RoleId.Returns((byte)3);
            userSession.ServerId.Returns(3001);
            userSession.NickName.Returns("alele1");
            var param = new LogoutContext(userSession);

            // Act & Assert
            Assert.IsTrue(1 == param.OnlineUserId
                && "master1" == param.UserName
                && 3 == param.RoleId
                && 3001 == param.ServerId
                && "alele1" == param.NickName
                && " {KickOutSessionChanged}" == param.Reason);
        }
    }
}
