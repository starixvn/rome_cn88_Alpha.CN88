namespace Alpha.CN88.Tests.Criteria
{
    using Alpha.CN88.Criteria;
    using NUnit.Framework;

    [TestFixture]
    public class SuggestionCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var suggestionCriteria = new SuggestionCriteria();
            var expectedKey = "Age_CustomerSelUserSearch";

            // Act
            var settingKey = suggestionCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedKey, settingKey);
        }

        [TestCase(0)]
        [TestCase(-2)]
        public void IsValid_CusIdNotPossitive_ReturnsFalse(int cusIdValue)
        {
            // Arrange
            var suggestionCriteria = new SuggestionCriteria()
            {
                UserId = 2,
                CustId = cusIdValue,
                IsInternalAgent = false,
                Username = "member01",
                IsSubAccount = false,
                ExcludeMember = false,
                DirectDownline = false,
                IsAccInfo = false
            };

            // Act
            var valid = suggestionCriteria.IsValid();

            // Assert
            Assert.IsFalse(valid);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("                     ")]
        public void IsValid_InValidUsername_ReturnsFalse(string username)
        {
            // Arrange
            var suggestionCriteria = new SuggestionCriteria()
            {
                UserId = 2,
                CustId = 12,
                IsInternalAgent = false,
                Username = username,
                IsSubAccount = false,
                ExcludeMember = false,
                DirectDownline = false,
                IsAccInfo = false
            };

            // Act
            var valid = suggestionCriteria.IsValid();

            // Assert
            Assert.IsFalse(valid);
        }

        [Test]
        public void IsValid_CusIdPossitiveAndValidUserName_ReturnsTrue()
        {
            // Arrange
            var suggestionCriteria = new SuggestionCriteria()
            {
                UserId = 2,
                CustId = 99,
                IsInternalAgent = false,
                Username = "member01",
                IsSubAccount = false,
                ExcludeMember = false,
                DirectDownline = false,
                IsAccInfo = false
            };

            // Act
            var valid = suggestionCriteria.IsValid();

            // Assert
            Assert.IsTrue(valid);
        }

    }
}
