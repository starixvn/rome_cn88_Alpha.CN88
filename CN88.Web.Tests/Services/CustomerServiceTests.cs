namespace CN88.Web.Tests.Services
{
    using System;
    using CN88.Web.Criteria;
    using CN88.Web.Models;
    using CN88.Web.Services;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class CustomerServiceTests
    {
        private const string IsCustomerActiveCacheKey = "CN88.Web.Services.IsAccountActiveCacheKey_{0}_{1}";
        private const string RecheckinCacheKey = "CN88.Web.Services.RecheckinCacheKey_{0}_{1}_{2}";

        private CacheService cacheService;
        private IDynamicRepository dynamicRepository;
        private CustomerService customerService;

        [SetUp]
        public void SetUp()
        {
            cacheService = Substitute.For<CacheService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            customerService = new CustomerServiceImpl(cacheService, dynamicRepository);
        }

        [Test]
        public void IsSubAccBelonging_ContainsSubAccount_ReturnsTrue()
        {
            // Arrange
            var custId = 1;
            var subAccId = 2;
            dynamicRepository
                .Fetch<int>(Arg.Is<GetSubAccountListCriteria>(c => c.CustId == custId))
                .Returns(new int[] { 1, 2, 3 });

            // Act
            var actual = customerService.IsSubAccBelonging(custId, subAccId);

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsSubAccBelonging_NotContainSubAccount_ReturnsFalse()
        {
            // Arrange
            var custId = 1;
            var subAccId = 10;
            dynamicRepository
                .Fetch<int>(Arg.Is<GetSubAccountListCriteria>(c => c.CustId == custId))
                .Returns(new int[] { 1, 2, 3 });

            // Act
            var actual = customerService.IsSubAccBelonging(custId, subAccId);

            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void GetBelongingCustRoleId_Always_CallDynamicRepositoryExecute()
        {
            // Arrange
            var actorId = 1;
            var actorSubAccountId = 0;
            var custId = 2;

            // Act
            customerService.GetBelongingCustRoleId(actorId, actorSubAccountId, custId);

            // Assert
            dynamicRepository.Received().Execute(
                Arg.Is<CheckBelongingCommand>(
                    c => c.SessionCustId == actorId
                        && c.SubAccountId == actorSubAccountId
                        && c.CustId == custId));
        }

        [Test]
        public void GetHeadlineBelongingCustRoleId_Always_CallDynamicRepositoryExecute()
        {
            // Arrange
            var actorId = 1;
            var custId = 2;

            // Act
            customerService.GetHeadlineBelongingCustRoleId(actorId, custId);

            // Assert
            dynamicRepository.Received().Execute(
                Arg.Is<CheckHeadlineBelongingCommand>(
                    c => c.SessionCustId == actorId
                        && c.CustId == custId));
        }

        [Test]
        public void CheckCustomerStatus_IsSubAccount_ReturnsCorrectModel()
        {
            // Arrange
            var custId = 2;
            var subAccId = 3;
            var customerStatus = new CustomerStatus();
            dynamicRepository
                .Get<CustomerStatus>(
                    Arg.Is<CheckCustomerStatusCriteria>(c => c.CustId == custId && c.SubAccId == subAccId))
                .Returns(customerStatus);

            // Act
            var actual = customerService.CheckCustomerStatus(custId, subAccId);

            // Assert
            Assert.IsTrue(actual.IsSubAccount);
        }

        [Test]
        public void CheckCustomerStatus_InCache_GetDataFromCache()
        {
            // Arrange
            var custId = 1;
            var subAccId = 0;
            var cacheInSeconds = 5;
            CustomerStatus customerStatus;
            cacheService.TryGet(
                string.Format(IsCustomerActiveCacheKey, custId, subAccId),
                out customerStatus).Returns(true);

            // Act
            customerService.CheckCustomerStatus(custId, subAccId, cacheInSeconds);

            // Assert
            dynamicRepository.DidNotReceive().Get<CustomerStatus>(
                Arg.Is<CheckCustomerStatusCriteria>(
                    c => c.CustId == custId && c.SubAccId == subAccId));
        }

        [Test]
        public void CheckCustomerStatus_NotInCache_CallsSetCacheSuccessfully()
        {
            // Arrange
            var custId = 1;
            var subAccId = 0;
            var cacheInSeconds = 5;
            CustomerStatus customerStatus;
            cacheService.TryGet(
                string.Format(IsCustomerActiveCacheKey, custId, subAccId),
                out customerStatus).Returns(false);
            dynamicRepository
                .Get<CustomerStatus>(
                    Arg.Is<CheckCustomerStatusCriteria>(c => c.CustId == custId && c.SubAccId == subAccId))
                .Returns(new CustomerStatus());

            // Act
            customerService.CheckCustomerStatus(custId, subAccId, cacheInSeconds);

            // Assert
            cacheService.Received().Set(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<CacheItemOptions>());
        }

        [Test]
        public void IsKickedOut_InvalidOnlineUserId_ReturnsTrue()
        {
            // Arrange
            var onlineUserId = -1;

            // Act
            var actual = customerService.IsKickedOut(onlineUserId);

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsKickedOut_ValidOnlineUserId_CallsDynamicRepository()
        {
            // Arrange
            var onlineUserId = 1;

            // Act
            customerService.IsKickedOut(onlineUserId);

            // Assert
            dynamicRepository.Received().Execute(Arg.Is<ReCheckinCommand>(c => c.OnlineUserId == onlineUserId));
        }

        [Test]
        public void Logout_Always_CallsDynamicRepository()
        {
            // Arrange
            IUserSession userSession = Substitute.For<IUserSession>();
            userSession.OnUserId.Returns(1);
            userSession.IsSubAcc.Returns(false);
            userSession.UserName.Returns("super");
            userSession.RoleId.Returns((byte)4);
            userSession.ServerId.Returns(3001);
            userSession.NickName.Returns("super_nickname");
            var logoutContext = new LogoutContext(userSession);

            // Act
            customerService.Logout(logoutContext);

            // Assert
            dynamicRepository.Received().Execute(Arg.Any<LogoutCommand>());
        }
    }
}
