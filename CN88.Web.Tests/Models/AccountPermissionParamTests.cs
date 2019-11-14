namespace CN88.Web.Tests.Models
{
    using CN88.Web.Models;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class AccountPermissionParamTests
    {
        private const string CustomerStatusSessionKey = "CUSTOMERSTATUS";
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
            userSession.GetProperty<byte>(CustomerStatusSessionKey).Returns((byte)2);
            userSession.SubPageAccess.Returns((PageAccess)"[A][B][C][D]");
            var param = new AccountPermissionParam("/", true, userSession);

            // Act & Assert
            Assert.IsTrue(2 == param.ActorStatus
                && "[A][B][C][D]" == (string)param.ActorSubPageAccess
                && param.IsHttpPost
                && param.ActorStatusIsNotActive
                && !param.ActorIsMainAccount);
        }
    }
}
