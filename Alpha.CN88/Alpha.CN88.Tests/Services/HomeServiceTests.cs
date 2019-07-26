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
        private IDynamicRepository homeRepository;
        private HomeServiceImpl homeService;

        [SetUp]
        public void SetUp()
        {
            homeRepository = Substitute.For<IDynamicRepository>();
            homeService = new HomeServiceImpl(homeRepository);
        }

        [Test]
        public void GetGeneralInformation_Always_CallFetchMultiple()
        {
            // Arrange
            var period = new AccountingPeriod();
            homeRepository
                .Fetch<AccountingPeriod>(Arg.Any<GetAccountingPeriodsCriteria>())
                .Returns(new List<AccountingPeriod> { period });

            // Act
            homeService.GetGeneralInformation(998, 0, false);

            // Assert
            homeRepository.Received(1).FetchMultiple<AccountOverview, StatusSummary>(Arg.Any<GetHomeInformationCriteria>());
        }
    }
}
