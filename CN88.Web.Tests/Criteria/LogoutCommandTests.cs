using CN88.Web.Criteria;
using CN88.Web.Models;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace CN88.Web.Tests.Criteria
{
    [TestFixture]
    public class LogoutCommandTests
    {
        private IUserSession userSession;

        [SetUp]
        public void SetUp()
        {
            userSession = Substitute.For<IUserSession>();
        }

        [Test]
        public void GetSettingKey_Always_ReturnsCorrectValue()
        {
            var command = new LogoutCommand(new LogoutContext(userSession));

            Assert.AreEqual("Age_CheckOut", command.GetSettingKey());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidOnlineUserId_ReturnsFalse(int onlineUserId)
        {
            userSession.OnUserId.Returns(onlineUserId);
            var command = new LogoutCommand(new LogoutContext(userSession));

            Assert.IsFalse(command.IsValid());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_InvalidUsername_ReturnsFalse(string username)
        {
            userSession.OnUserId.Returns(1);
            userSession.UserName.Returns(username);
            var command = new LogoutCommand(new LogoutContext(userSession));

            Assert.IsFalse(command.IsValid());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsValid_InvalidServerId_ReturnsFalse(int serverid)
        {
            userSession.OnUserId.Returns(1);
            userSession.UserName.Returns("username");
            userSession.ServerId.Returns(serverid);
            var command = new LogoutCommand(new LogoutContext(userSession));

            Assert.IsFalse(command.IsValid());
        }
    }
}