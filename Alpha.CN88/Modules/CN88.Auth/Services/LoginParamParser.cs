namespace CN88.Auth.Services
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using Fanex.Cryptography.Aes;
    using Fanex.Logging.Extensions.Web;
    using Params.Authentication;
    using RSACryptoPad;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Utilities;
    using Sunplus.Core.Common.IPs;
    using UAParser;

    public interface LoginParamParser
    {
        string[] DecryptString(string requestParam, string ibckeyPath);

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

        private static readonly string[] ValidLanguages = { "en-US", "zh-Hans", "zh-TW" };
        private static readonly string[] ValidLanguageKeys = { "E", "CN", "CT" };

        public string[] DecryptString(string requestParam, string ibckeyPath)
        {
            var encryption = new EncryptionThread();

            try
            {
                requestParam = encryption.DecryptString(requestParam.Replace(' ', '+'), DWKeySize, ibckeyPath);
            }
            catch
            {
                ExceptionHandler.Handle(new Exception("Cannot verify request key " + GetRequest().RawUrl));
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
                ExceptionHandler.Handle(
                    new Exception("Verify customer information failed. " + string.Join(";", keyValuePairs)),
                    TraceEventType.Information);

                return new LoginParam();
            }

            var password = AesExtension.Decrypt(keyValuePairs[PasswordIndex], keyValuePairs[EncryptCodeIndex]);

            bool isNickName;
            bool.TryParse(keyValuePairs[IsNickNameIndex], out isNickName);

            var language = keyValuePairs[LanguageIndex];
            var validLanguageIndex = Array.FindIndex(ValidLanguages, item => item.Equals(language, StringComparison.OrdinalIgnoreCase));
            var languageKey = validLanguageIndex > 0 ? ValidLanguageKeys[validLanguageIndex] : ValidLanguageKeys[0];

            var deviceId = keyValuePairs[DeviceIndex];

            return new LoginParam(
                custId,
                Encryption.CfsEnCode(password),
                subAccId,
                timeTicks,
                keyValuePairs[NickNameIndex],
                isNickName,
                language,
                languageKey,
                GetRequestInfo(GetRequest(), keyValuePairs[BrowserSizeIndex]),
                string.IsNullOrWhiteSpace(deviceId) ? "N/A" : deviceId);
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

            return new AdminLoginParam(
                adminId,
                loginParamsLength > 3 ? keyValuePairs[3] : DefaultSystemLogin,
                custName,
                timeTicks,
                ValidLanguages[0],
                ValidLanguageKeys[0],
                GetRequestInfo(GetRequest(), loginParamsLength >= 11 ? keyValuePairs[BrowserSizeIndex] : string.Empty));
        }

        private static HttpRequest GetRequest()
            => HttpContext.Current.Request;

        private static RequestInfo GetRequestInfo(HttpRequest request, string browserSize)
        {
            var ip = AgentIPFormat.GetClientIP();
            var browserName = string.Empty;
            var platform = string.Empty;
            
            try
            {
                var userAgentParser = Parser.GetDefault();
                var clientInfo = userAgentParser.Parse(request.UserAgent ?? string.Empty);

                browserName = GetBrowserName(clientInfo.UserAgent, browserSize);
                platform = GetPlatform(clientInfo);
            }
            catch
            {
                ExceptionHandler.Handle(new Exception("Cannot get request info"), TraceEventType.Information);
            }

            return new RequestInfo(
                ip,
                request.ServerVariables["SERVER_NAME"],
                browserName,
                platform,
                GetCountryCode(ip));
        }

        private static string GetCountryCode(string ip)
        {
            var country = "Unknown";

            try
            {
                var infoIp = IpUtils.LocationFromIP(ip);
                country = infoIp.CountryLong == "Reserved" ? null : infoIp.CountryLong;
            }
            catch
            {
                ExceptionHandler.Handle(new Exception("Cannot get country info"), TraceEventType.Information);
            }

            return country;
        }

        private static string GetBrowserName(UserAgent userAgent, string browserSize)
        {
            var browser = $"{userAgent.Family} {userAgent.Major}.{userAgent.Minor}.{userAgent.Patch ?? "0"}";

            return string.IsNullOrWhiteSpace(browserSize)
                ? browser
                : browser + $", {browserSize}";
        }

        private static string GetPlatform(ClientInfo clientInfo)
        {
            var deviceInfo = clientInfo.Device;
            var device = $"{deviceInfo.Family}, IsCrawler: {deviceInfo.IsSpider}";

            var os = clientInfo.OS;
            var majorVersion = os.Major ?? "0";
            var minorVersion = os.Minor ?? "0";
            var osVersion = os.Patch ?? "0";
            var operateSystem = $"{os.Family} {majorVersion}.{minorVersion}.{osVersion}";

            return $"{operateSystem}, Device: {device}";
        }
    }
}
