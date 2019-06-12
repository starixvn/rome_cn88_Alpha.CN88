namespace Alpha.CN88.Tests.Services
{
    using System.Collections.Generic;
    using Alpha.CN88.Criteria.Home;
    using Alpha.CN88.Models.Home;
    using Alpha.CN88.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HomeServiceTests
    {
        IDynamicRepository repository;

        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<IDynamicRepository>();
            var period = new AccountingPeriod();
            repository
                .Fetch<AccountingPeriod>(Arg.Any<GetAccountingPeriodsCriteria>())
                .Returns(new List<AccountingPeriod> { period });
        }

        [Test]
        public void GetGeneralInformation_Always_CallFetchMultiple()
        {
            // Arrange
            var service = new HomeServiceImpl(repository);

            // Act
            service.GetGeneralInformation(999);

            // Assert
            repository.Received(1).FetchMultiple<AccountOverview, StatusSummary>(Arg.Any<GetHomeInformationCriteria>());
        }
    }
}
