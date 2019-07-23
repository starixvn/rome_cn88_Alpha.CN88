namespace Alpha.CN88.Tests.Services
{
    using Alpha.CN88.Criteria;
    using Alpha.CN88.Services.Impl;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultServiceTests
    {
        private IDynamicRepository defaultRepository;

        [SetUp]
        public void SetUp()
        {
            defaultRepository = Substitute.For<IDynamicRepository>();
        }

        [Test]
        public void GetOnlineMember_Alway_CallRepositoryGetTotalMember()
        {
            // Arrange
            var comapareLogic = new CompareLogic();
            var criteria = new GetTotalMemberOnlineCriteria(999, 0, false);
            defaultRepository.Get<int>(Arg.Is<GetTotalMemberOnlineCriteria>(c => comapareLogic.Compare(c, criteria).AreEqual)).Returns(10);
            var service = new DefaultServiceImpl(defaultRepository);
            var expectedResult = 10;

            // Act
            var ressult = service.GetOnlineMember(999, 0, false);

            // Assert
            Assert.AreEqual(expectedResult, ressult);
        }
    }
}