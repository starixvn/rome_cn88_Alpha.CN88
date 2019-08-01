namespace Alpha.CN88.Tests.Services
{
    using Alpha.CN88.Criteria;
    using Alpha.CN88.Services.Impl;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultServiceTests
    {
        private IDynamicRepository defaultRepository;
        private DefaultServiceImpl defaultService;

        [SetUp]
        public void SetUp()
        {
            defaultRepository = Substitute.For<IDynamicRepository>();
            defaultService = new DefaultServiceImpl(defaultRepository);
        }

        [Test]
        public void GetOnlineMember_Alway_CallRepositoryGetTotalMember()
        {
            // Arrange
            defaultRepository.Get<int>(Arg.Any<GetTotalMemberOnlineCriteria>()).Returns(10);
            var expectedResult = 10;

            // Act
            var ressult = defaultService.GetOnlineMember(999, 0, false);

            // Assert
            Assert.AreEqual(expectedResult, ressult);
        }
    }
}
