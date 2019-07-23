namespace Alpha.CN88.Tests.Services
{
    using System;
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
        [ThreadStatic]
        IDynamicRepository homeRepository;

        [SetUp]
        public void SetUp()
        {
            homeRepository = Substitute.For<IDynamicRepository>();
            var period = new AccountingPeriod();
            homeRepository
                .Fetch<AccountingPeriod>(Arg.Any<GetAccountingPeriodsCriteria>())
                .Returns(new List<AccountingPeriod> { period });
        }

        [Test]
        public void GetGeneralInformation_Always_CallFetchMultiple()
        {
            // Arrange
            var service = new HomeServiceImpl(homeRepository);

            // Act
            service.GetGeneralInformation(998, 0, false);

            // Assert
            homeRepository.Received(1).FetchMultiple<AccountOverview, StatusSummary>(Arg.Any<GetHomeInformationCriteria>());
        }
    }
}