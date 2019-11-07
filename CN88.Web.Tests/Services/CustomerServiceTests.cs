namespace CN88.Web.Tests.Services
{
    using CN88.Web.Criteria;
    using CN88.Web.Models;
    using CN88.Web.Services;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerServiceTests
    {
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
    }
}
