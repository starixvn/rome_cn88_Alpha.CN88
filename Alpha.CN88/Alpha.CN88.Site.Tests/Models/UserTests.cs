using Alpha.CN88.Site.Models;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;

namespace Alpha.CN88.Site.Tests.Models
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Constructor_Always_GetUserDataFromSession()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.CustId.Returns(1);
            userSession.UserName.Returns("username");
            userSession.NickName.Returns("nickname");
            userSession.RoleId.Returns((byte)2);
            userSession.Language.Returns("en-US");
            userSession.LangKey.Returns("en");
            userSession.IsInternal.Returns(false);
            userSession.SubAccId.Returns(3);
            userSession.IsSubAcc.Returns(true);
            userSession.SubUserName.Returns("subusername");
            userSession.UserLoginIp.Returns("127.0.0.1");
            userSession.GetProperty<bool>("ISALLDOWNLINE").Returns(true);
            userSession.GetProperty<byte>("CUSTOMERSTATUS").Returns((byte)1);
            userSession.OnUserId.Returns(2);
            userSession.ServerId.Returns(3);
            userSession.IsLogged.Returns(true);
            userSession.IsPasswordExpired.Returns(false);
            userSession.IsForcedSecurityCode.Returns(false);
            userSession.SecurityCode.Returns("12345");
            userSession.AdminName.Returns("adminname");
            userSession.GetProperty<bool>("IsHideTopMenu").Returns(false);

            var pageAccess = new PageAccess() { UpdateAccount = true, ViewMemberInfo = true };
            userSession.SubPageAccess.Returns(pageAccess);

            // Act
            var user = new User(userSession);

            // Assert
            Assert.True(
                user.Id == 1
                && user.Name == "username"
                && user.NickName == "nickname"
                && user.LevelId == 2
                && user.Language == "en-US"
                && user.LanguageKey == "en"
                && !user.IsInternal
                && user.SubAccountId == 3
                && user.IsSubAcc
                && user.SubUserName == "subusername"
                && user.IP == "127.0.0.1"
                && user.IsAllDownline
                && user.CustomerStatus == 1
                && user.AccountManagementEditView
                && user.AccountManagementViewOnly
                && user.OnlineUserId == 2
                && user.ServerId == 3
                && user.IsLogged
                && !user.IsPasswordExpired
                && !user.IsForcedSecurityCode
                && user.SecurityCode == "12345"
                && user.AdminName == "adminname"
                && !user.IsHideTopMenu);
        }
    }
}