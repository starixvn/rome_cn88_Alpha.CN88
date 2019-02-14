namespace Alpha.CN88.Services.Authentication
{
    using System;
    using Fanex.Cryptography.Aes;
    using Params.Authentication;
    using RSACryptoPad;

    public interface LoginParamParser
    {
        string[] ToStringArray(string requestParam, string ibckeyPath);

        LoginParam ToObject(string[] keyValuePairs);

        AdminLoginParam ToAdminObject(string[] keyValuePairs);
    }

    public class LoginParamParserImpl : LoginParamParser
    {
        private const int DWKeySize = 1024;

        private const int CustIdIndex = 0;
        private const int SubAccIdIndex = 1;
        private const int TimeTicksIndex = 2;
        private const int PasswordIndex = 3;
        private const int NickNameIndex = 4;
        private const int IsNickNameIndex = 5;
        private const int LanguageIndex = 7;
        private const int DeviceIndex = 9;
        private const int BrowserSizeIndex = 10;
        private const int EncryptCodeIndex = 11;

        private const int AdminCustNameIndex = 0;
        private const int AdminIdIndex = 1;
        private const int AdminTimeTicksIndex = 2;
        private const string DefaultSystemLogin = "NAP";

        private static readonly string[] ValidLanguages = new[] { "en-US", "zh-CN", "zh-TW" };
        private static readonly string[] ValidLanguageCodes = new[] { "E", "CS", "CT" };

        public string[] ToStringArray(string requestParam, string ibckeyPath)
        {
            var encryption = new EncryptionThread();

            try
            {
                requestParam = encryption.DecryptString(requestParam.Replace(' ', '+'), DWKeySize, ibckeyPath);
            }
            catch
            {
                return new string[0];
            }

            return requestParam.Split('&');
        }

        public LoginParam ToObject(string[] keyValuePairs)
        {
            // LoginParams: custId, subAccId, loginTime, password, nickName, isNickName, __utms, language, no cross cookie.
            int custId, subAccId;
            long timeTicks;
            var loginParamsLength = keyValuePairs.Length;

            if (loginParamsLength < 12
                || !int.TryParse(keyValuePairs[CustIdIndex], out custId)
                || !int.TryParse(keyValuePairs[SubAccIdIndex], out subAccId)
                || !long.TryParse(keyValuePairs[TimeTicksIndex], out timeTicks))
            {
                return new LoginParam();
            }

            var loginTime = new DateTime(timeTicks);
            var password = AesExtension.Decrypt(keyValuePairs[PasswordIndex], keyValuePairs[EncryptCodeIndex]);

            bool isNickName;
            bool.TryParse(keyValuePairs[IsNickNameIndex], out isNickName);

            var language = keyValuePairs[LanguageIndex];
            var validLanguageIndex = Array.FindIndex(ValidLanguages, item => item.Equals(language, StringComparison.OrdinalIgnoreCase));
            var languageCode = validLanguageIndex > 0 ? ValidLanguageCodes[validLanguageIndex] : string.Empty;

            var deviceId = keyValuePairs[DeviceIndex];

            return new LoginParam(
                custId,
                subAccId,
                timeTicks,
                loginTime,
                password,
                keyValuePairs[NickNameIndex],
                isNickName,
                language,
                languageCode,
                string.IsNullOrWhiteSpace(deviceId) ? "N/A" : deviceId,
                keyValuePairs[BrowserSizeIndex]);
        }

        public AdminLoginParam ToAdminObject(string[] keyValuePairs)
        {
            // AdminLoginParams: custName, adminId, loginTime, systemLogin: optional.
            int adminId;
            long timeTicks;
            var custName = keyValuePairs[AdminCustNameIndex];
            var loginParamsLength = keyValuePairs.Length;

            if (loginParamsLength < 2
                || string.IsNullOrEmpty(custName)
                || !int.TryParse(keyValuePairs[AdminIdIndex], out adminId)
                || !long.TryParse(keyValuePairs[AdminTimeTicksIndex], out timeTicks))
            {
                return new AdminLoginParam();
            }

            var loginTime = new DateTime(timeTicks);
            var systemLogin = loginParamsLength > 3 ? keyValuePairs[3] : DefaultSystemLogin;

            return new AdminLoginParam(
                adminId,
                systemLogin,
                custName,
                timeTicks,
                loginTime,
                ValidLanguages[0],
                ValidLanguageCodes[0],
                loginParamsLength >= 11 ? keyValuePairs[BrowserSizeIndex] : string.Empty);
        }
    }
}
