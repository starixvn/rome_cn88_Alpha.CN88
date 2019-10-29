using Alpha.CN88.Models;
using Alpha.CN88.Site.Handlers;
using Alpha.CN88.Site.Requests.SecurityCode;
using Alpha.CN88.Site.ViewModels.SecurityCode;
using Alpha.SecurityCode.Web;
using CN88.Auth.Models;
using CN88.Auth.Params.SecurityCode;
using CN88.Auth.Services;
using CN88.Web.Helpers;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using NUnit.Framework;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Utilities;

namespace Alpha.CN88.Site.Tests.Handlers
{
    [TestFixture]
    public class SecurityCodeHandlerTests
    {
        private SessionManager sessionManager;
        private IUserSession userSession;
        private Encryptor encryptor;
        private IUser user;
        private SecurityCodeService securityCodeService;
        private SecurityCodeHandler handler;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            encryptor = Substitute.For<Encryptor>();
            encryptor.RandomString(Arg.Any<int>()).Returns("HashKey");

            sessionManager = Substitute.For<SessionManager>();
            sessionManager.Get<string>(Arg.Is("SECURITYCODETOKEN")).Returns("SecurityCode");

            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();

            securityCodeService = Substitute.For<SecurityCodeService>();
            securityCodeService.GenerateSecurityToken().Returns("SecurityCodeToken");
            securityCodeService.GenerateSalt(Arg.Any<int>()).Returns("SecuritySalt");

            userSession = Substitute.For<IUserSession>();
            userSession.RoleId.Returns((byte)3);

            user = Substitute.For<IUser>();

            handler = new SecurityCodeHandler(sessionManager, userSession, encryptor, securityCodeService);
            compareLogic = new CompareLogic();
        }

        [Test]
        public void HandleChangeSecurityCodeCommand_IsMainAccount_CallsChangeMainAccountSecurityCode()
        {
            // arrange
            user.SubAccountId.Returns(0);
            user.IsInternal.Returns(false);
            user.SecurityCode.Returns("1234");
            user.IsForcedSecurityCode.Returns(true);
            var message = new ChangeSecurityCodeCommand(user);
            securityCodeService.ChangeMainAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>())
                .Returns(new SecurityResult());

            // act
            handler.Handle(message);

            // assert
            securityCodeService.Received().ChangeMainAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    p => p.CustId == message.ViewerId
                        && p.SubAccId == message.SubAccountId
                        && p.CurrentSecurityCode == message.OldSecurityCode
                        && p.Salt == "SecurityCode"
                        && p.UserId == message.ViewerId
                        && p.IP == message.IP
                        && p.LanguageKey == message.Language));
        }

        [Test]
        public void HandleChangeSecurityCodeCommand_IsSubAccount_CallsChangeMainAccountSecurityCode()
        {
            // arrange
            user.SubAccountId.Returns(1);
            user.IsInternal.Returns(false);
            user.IsForcedSecurityCode.Returns(true);
            var message = new ChangeSecurityCodeCommand(user);
            securityCodeService.ChangeSubAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>())
                .Returns(new SecurityResult());

            // act
            handler.Handle(message);

            // assert
            securityCodeService.Received().ChangeSubAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    p => p.CustId == message.ViewerId
                        && p.SubAccId == message.SubAccountId
                        && p.CurrentSecurityCode == message.OldSecurityCode
                        && p.Salt == "SecurityCode"
                        && p.UserId == message.ViewerId
                        && p.IP == message.IP
                        && p.LanguageKey == message.Language
                        && p.MustVerifyCurrentSecurityCode));
        }

        [Test]
        public void HandleResetSecurityCodeCommand_IsNotForceChangeFirstLogin_CallsChangeMainAccountSecurityCode()
        {
            // arrange
            user.SubAccountId.Returns(1);
            user.IsInternal.Returns(false);
            user.IsForcedSecurityCode.Returns(false);
            var message = new ResetSecurityCodeCommand(user);
            securityCodeService.ChangeSubAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>())
                .Returns(new SecurityResult());

            // act
            handler.Handle(message);

            // assert
            securityCodeService.Received().ChangeMainAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    p => p.CustId == message.CustId
                        && p.SubAccId == message.SubAccountId
                        && p.UserId == message.ViewerId
                        && p.IP == message.IP
                        && p.LanguageKey == message.Language));
        }

        [Test]
        public void HandleResetSecurityCodeCommand_IsForceChangeFirstLogin_IsMainAccount_CallsChangeMainAccountSecurityCode()
        {
            // arrange
            user.SubAccountId.Returns(0);
            user.IsInternal.Returns(false);
            user.IsForcedSecurityCode.Returns(true);
            var message = new ResetSecurityCodeCommand(user);
            securityCodeService.ChangeMainAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>())
                .Returns(new SecurityResult());

            // act
            handler.Handle(message);

            // assert
            securityCodeService.Received().ChangeMainAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    p => p.CustId == message.CustId
                        && p.SubAccId == message.SubAccountId
                        && p.UserId == message.ViewerId
                        && p.IP == message.IP
                        && p.LanguageKey == message.Language));
        }

        [Test]
        public void HandleResetSecurityCodeCommand_IsForceChangeFirstLogin_IsSubAccount_CallsChangeMainAccountSecurityCode()
        {
            // arrange
            user.SubAccountId.Returns(1);
            user.IsInternal.Returns(false);
            user.IsForcedSecurityCode.Returns(true);
            var message = new ResetSecurityCodeCommand(user);
            securityCodeService.ChangeSubAccountSecurityCode(Arg.Any<ChangeSecurityCodeParam>())
                .Returns(new SecurityResult());

            // act
            handler.Handle(message);

            // assert
            securityCodeService.Received().ChangeSubAccountSecurityCode(
                Arg.Is<ChangeSecurityCodeParam>(
                    p => p.CustId == message.CustId
                        && p.SubAccId == message.SubAccountId
                        && p.UserId == message.ViewerId
                        && p.IP == message.IP
                        && p.LanguageKey == message.Language
                        && !p.MustVerifyCurrentSecurityCode));
        }

        [Test]
        public void HandleResetSubAccountSecurityCodeCommand_Always_CalsResetSubAccountSecurityCode()
        {
            // arrange
            var message = new ResetSubAccountSecurityCodeCommand(user);

            // act
            handler.Handle(message);

            // assert
            securityCodeService.Received().ResetSubAccountSecurityCode(
                Arg.Is<ResetSecurityCodeParam>(
                    p => p.CustId == message.ViewerId
                        && p.SubAccId == message.CustId
                        && p.UserId == message.ViewerId
                        && p.IP == message.IP
                        && p.LanguageKey == message.Language));
        }

        [Test]
        public void HandleVerifySecurityCodeCommand_Always_CalsVerifySecurityCode()
        {
            // arrange
            var message = new VerifySecurityCodeCommand(user);
            securityCodeService.VerifySecurityCode(Arg.Any<VerifySecurityCodeParam>())
                .Returns(new SecurityResult());

            // act
            handler.Handle(message);

            // assert
            securityCodeService.Received().VerifySecurityCode(
                Arg.Is<VerifySecurityCodeParam>(
                    p => p.CustId == message.ViewerId
                        && p.SubAccId == message.SubAccountId
                        && p.SecurityCode == message.SecurityCode
                        && p.Salt == "SecurityCode"
                        && p.IP == message.IP
                        && p.LanguageKey == message.Language));
        }

        [Test]
        public void HandleChangeSecurityCodeQuery_Always_ReturnsCorrectViewModel()
        {
            // arrange
            var message = new ChangeSecurityCodeQuery(user);
            securityCodeService.GenerateSecurityToken().Returns("SCToken");
            securityCodeService.GenerateSalt(Arg.Any<int>()).Returns("Salt");
            var expectedViewModel = new ChangeSecurityCodeViewModel(message.ViewerName, "SCToken", "Salt", "HashKey", message.Result);

            // act
            var actualViewModel = handler.Handle(message);

            // assert
            Assert.IsTrue(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Test]
        public void HandleResetSecurityCodeQuery_Always_ReturnsCorrectViewModel()
        {
            // arrange
            var message = new ResetSecurityCodeQuery(user);
            var expectedViewModel = new ResetSecurityCodeViewModel(message.CustId, message.CustName, "HashKey", message.Result);

            // act
            var actualViewModel = handler.Handle(message);

            // assert
            Assert.IsTrue(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Test]
        public void HandleVerifySecurityCodeQuery_Always_ReturnsCorrectViewModel()
        {
            // arrange
            var message = new VerifySecurityCodeQuery(user);
            securityCodeService.GenerateSecurityToken().Returns("SCToken");
            securityCodeService.GenerateSalt(Arg.Any<int>()).Returns("Salt");
            var expectedViewModel = new VerifySecurityCodeViewModel("SCToken", "Salt");

            // act
            var actualViewModel = handler.Handle(message);

            // assert
            Assert.IsTrue(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}