namespace Alpha.CN88.Tests.Configurations
{
    using System.Collections.Specialized;
    using Alpha.CN88.Configurations;
    using NUnit.Framework;

    [TestFixture]
    public class AppSettingsTests
    {
        [SetUp]
        public void SetUp()
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["StoredProcedurePath"] = "Sentry",
                ["AssetAppPath"] = "app",
                ["FeaturesAppPath"] = "feature",
                ["MessageAppPath"] = "host",
                ["TotalBetAppPath"] = "http:",
                ["IntervalToGetTotalOnlineMember"] = "12",
                ["URL:MemberSite"] = "t,,tu;i",
                ["URL:MemberSiteMobile"] = "12,1,t;o;;,,",
                ["URL:AgentSiteLink"] = "1,,t,hht:,;k;;j",
                ["HideSubAccountMenu"] = "1",
                ["LivePersonAccountNumber"] = "123"
            };
        }

        [Test]
        public void Verify_HasEnoughSettings_PassVerify()
        {
            var passVerify = false;

            // Act
            AppSettings.Verify();
            passVerify = true;

            // Assert
            Assert.True(passVerify);
        }

        [Test]
        public void GetStoredProcedurePath_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = "Sentry";

            // Act
            var result = AppSettings.StoreProcedurePath;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetAssetAppPath_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = "app";

            // Act
            var result = AppSettings.AssetAppPath;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetFeaturesAppPath_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = "feature";

            // Act
            var result = AppSettings.FeaturesAppPath;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetMessageAppPath_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = "host";

            // Act
            var result = AppSettings.MessageAppPath;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetTotalBetAppPath_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = "http:";

            // Act
            var result = AppSettings.TotalBetAppPath;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetIntervalToGetTotalOnlineMember_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = 12;

            // Act
            var result = AppSettings.IntervalToGetTotalOnlineMember;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetURLMemberSite_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = new[] { "t", "tu", "i" };

            // Act
            var result = AppSettings.MemberSiteLinks;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetURLMemberSiteMobile_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = new[] { "12", "1", "t", "o" };

            // Act
            var result = AppSettings.MemberSiteMobileLinks;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetURLAgentSiteLink_Always_ReturnCorrectSetting()
        {
            // Arrange
            var expectedValue = new[] { "1", "t", "hht:", "k", "j" };

            // Act
            var result = AppSettings.AgentSiteLinks;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [TestCase("StoredProcedurePath", "Sentry")]
        [TestCase("UnexistValue", null)]
        [TestCase("", null)]
        [TestCase(null, null)]
        public void GetAppPath_Always_ReturnCorrectSetting(string settingName, string expectValue)
        {
            // Act
            var result = AppSettings.GetAppPath(settingName);

            // Assert
            Assert.AreEqual(expectValue, result);
        }

        [Test]
        public void HideSubAccountMenu_Always_ReturnCorrectSetting()
        {
            // Act
            var result = AppSettings.HideSubAccountMenu;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void LivePersonAccountNumber_Always_ReturnCorrectSetting()
        {
            // Act
            var result = AppSettings.LivePersonAccountNumber;

            // Assert
            Assert.AreEqual("123", result);
        }
    }
}