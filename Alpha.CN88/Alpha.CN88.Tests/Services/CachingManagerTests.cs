namespace Alpha.CN88.Tests.Services
{
    using System;
    using System.Runtime.Caching;
    using Alpha.CN88.Services.Impl;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CachingManagerTests
    {
        CachingManagerImpl cachingManager;
        CacheItemPolicy policy;
        TimeSpan timeSpan;
        DateTimeOffset expireTime;
        Func<string> getStringValue;
        Func<int> getIntValue;

        [SetUp]
        public void SetUp()
        {
            cachingManager = new CachingManagerImpl();
            expireTime = DateTimeOffset.Now.AddSeconds(3600);
            policy = new CacheItemPolicy { AbsoluteExpiration = expireTime };
            timeSpan = expireTime.TimeOfDay;
            getStringValue = () => "temp";
            getIntValue = () => 12;
        }

        [Test]
        public void DefaultCacheDuration_Always_SetCorrectDefaultExpiryDateTime()
        {
            // Arrange
            var expectedValue = 1200;

            // Act
            var result = cachingManager.DefaultCacheDuration;

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetCacheItem_KeyNull_ThrowNullArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cachingManager.Get<string>(null));
        }

        [TestCase("")]
        [TestCase("      ")]
        public void GetCacheItem_KeyEmptyOrWhiteSpace_ArgumentOutOfRangeException(string key)
        {

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cachingManager.Get<string>(key));
        }

        [Test]
        public void GetCacheItem_InValidTypeCache_ReturnDefaultValueOfType()
        {
            // Arrange
            var key = "wta";
            var expectedValue = 0;
            cachingManager.Add(key, "value", policy);

            // Act
            var result = cachingManager.Get<int>(key);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetCacheItem_UnExistCache_ReturnDefaultValueOfType()
        {
            // Arrange
            var key = "wta";
            var expectedValue = 0;

            // Act
            var result = cachingManager.Get<int>(key);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [TestCase("wth", "value", "value")]
        public void GetCacheItem_ValidKeyCache_ReturnValueOfKey(string key, string keyValue, string expectedValue)
        {
            // Arrange
            cachingManager.Add(key, keyValue, policy);

            // Act
            var result = cachingManager.Get<string>(key);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetCacheItem_LazyTypeCache_ReturnValueOfKey()
        {
            // Arrange
            var key = "test";
            var expectedValue = "temp";
            var value = new Lazy<string>(() => expectedValue);
            cachingManager.Add(key, value, policy);

            // Act
            var result = cachingManager.Get<string>(key);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void ValidateKey_KeyNull_ThrowNullArgumentException()
        {
            // Arrange
            var temp = "test";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cachingManager.Add(null, temp, policy));
        }

        [TestCase("")]
        [TestCase("      ")]
        public void ValidateKey_KeyEmptyOrWhiteSpace_ArgumentOutOfRangeException(string key)
        {
            // Arrange
            var temp = "test";

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cachingManager.Add(key, temp, policy));
        }

        [Test]
        public void AddCacheItemPolicy_ItemNull_ThrowNullArgumentException()
        {
            // Arrange
            var validKey = "test";
            object tempObject = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cachingManager.Add(validKey, tempObject, policy));
        }

        [Test]
        public void AddCacheItemPolicy_ValidItem_ItemAddedToCache()
        {
            // Arrange
            var validKey = "test";
            var tempObject = "temp";
            cachingManager.Add(validKey, tempObject, policy);

            // Act
            var result = cachingManager.Get<string>(validKey);

            // Assert
            Assert.AreEqual(tempObject, result);
        }

        [Test]
        public void AddCacheItemSlidingExpiration_ItemNull_ThrowNullArgumentException()
        {
            // Arrange
            var validKey = "test";
            object tempObject = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cachingManager.Add(validKey, tempObject, timeSpan));
        }

        [Test]
        public void AddCacheItemSlidingExpiration_ValidKeyAndItem_ItemAddedToCache()
        {
            // Arrange
            var validKey = "test";
            var tempObject = "temp";
            cachingManager.Add(validKey, tempObject, timeSpan);

            // Act
            var result = cachingManager.Get<string>(validKey);

            // Assert
            Assert.AreEqual(tempObject, result);
        }

        [Test]
        public void AddCacheItemAbsoluteExpiration_ItemNull_ThrowNullArgumentException()
        {
            // Arrange
            var validKey = "test";
            object tempObject = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => cachingManager.Add(validKey, tempObject, expireTime));
        }

        [Test]
        public void AddCacheItemAbsoluteExpiration_ValidKeyAndItem_ItemAddedToCache()
        {
            // Arrange
            var validKey = "test";
            var tempObject = "temp";
            cachingManager.Add(validKey, tempObject, expireTime);

            // Act
            var result = cachingManager.Get<string>(validKey);

            // Assert
            Assert.AreEqual(tempObject, result);
        }

        [Test]
        public void AddCacheItemDefaultExpiryDateTime_ItemNull_ThrowNullArgumentException()
        {
            // Arrange
            var validKey = "test";
            object tempObject = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => cachingManager.Add(validKey, tempObject));
        }

        [Test]
        public void AddCacheItemDefaultExpiryDateTime_ValidKeyAndItem_ItemAddedToCache()
        {
            // Arrange
            var validKey = "test";
            var tempObject = "temp";
            cachingManager.Add(validKey, tempObject);

            // Act
            var result = cachingManager.Get<string>(validKey);

            // Assert
            Assert.AreEqual(tempObject, result);
        }

        [Test]
        public void GetOrAddCacheItemPolicy_NullFunction_ThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cachingManager.GetOrAdd<string>("temp", null, policy));
        }

        [Test]
        public void GetOrAddCacheItemPolicy_DoNotExistKey_ReturnCorrectValue()
        {
            // Arrange
            var expectedValue = "temp";

            // Act
            var result = cachingManager.GetOrAdd<string>("tt", getStringValue, policy);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetOrAddCacheItemPolicy_ExistedKey_ReturnCorrectValue()
        {
            // Arrange
            cachingManager.Add("tt", "khong");
            var expectedValue = "khong";

            // Act
            var result = cachingManager.GetOrAdd<string>("tt", getStringValue, policy);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetOrAddCacheItemPolicy_ExistedLazyKey_ReturnCorrectValue()
        {
            // Arrange
            Lazy<int> getLazyInt = new Lazy<int>(() => 4);
            cachingManager.Add("tt", getLazyInt);
            var expectedValue = 4;

            // Act
            var result = cachingManager.GetOrAdd<int>("tt", getIntValue, policy);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetOrAddCacheItemPolicy_ExistedKeyDifferType_ReturnCorrectValue()
        {
            // Arrange
            cachingManager.Add("tt", "what's up");
            var expectedValue = 0;

            // Act
            var result = cachingManager.GetOrAdd<int>("tt", getIntValue, policy);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void GetOrAddCacheItemPolicy_ErrorAddIemFactory_ThrowArgumentNullException()
        {
            // Arrange
            Func<int> throwMissigMemberException = () => { throw new MissingMemberException(); };
            TestDelegate testDelegate = () => cachingManager.GetOrAdd("GetOrAddCacheItemPolicyKey", throwMissigMemberException, policy);

            // Assert
            Assert.Throws<MissingMemberException>(testDelegate);
        }

        [Test]
        public void Remove_ValidKey_RemoveKeyInCache()
        {
            // Arrange
            cachingManager.Add("tt", "what's up");

            // Act
            cachingManager.Remove("tt");

            // Asset
            Assert.IsNull(cachingManager.Get<string>("tt"));
        }

        [Test]
        public void GetOrAddTwoParam_Always_CallGetOrAddWithDefaultExpiryDateTime()
        {
            // Arrange
            var sub = Substitute.ForPartsOf<CachingManagerImpl>();
            var mng = sub;

            // Act
            mng.GetOrAdd<int>("temp", getIntValue);

            // Assert
            mng.Received(1).GetOrAdd<int>("temp", getIntValue, DateTimeOffset.Now);
        }

        [Test]
        public void GetOrAddWithDateTimeOffset_Always_CallGetOrAddWithCachePolicy()
        {
            // Arrange
            var sub = Substitute.ForPartsOf<CachingManagerImpl>();
            var mng = sub;

            // Act
            mng.GetOrAdd<int>("temp", getIntValue, expireTime);

            // Assert
            mng.Received(1).GetOrAdd<int>("temp", getIntValue, new CacheItemPolicy());
        }

        [Test]
        public void GetOrAddWithTimespan_Always_CallGetOrAddWithCachePolicy()
        {
            // Arrange
            var sub = Substitute.ForPartsOf<CachingManagerImpl>();
            var mng = sub;

            // Act
            mng.GetOrAdd<int>("temp", getIntValue, timeSpan);

            // Assert
            mng.Received(1).GetOrAdd<int>("temp", getIntValue, new CacheItemPolicy());
        }
    }

}
