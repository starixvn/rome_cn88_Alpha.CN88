namespace Alpha.CN88.Site.App_Start
{
    using Configurations;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Logging.RabbitMQ;
    using Fanex.Logging.Sentry.RavenClient;
    using SharpRaven;
    using System.Collections.Generic;
    using System.Configuration;

    public static class LoggingConfig
    {
        public static void Init()
        {
            LogManager
                 .SetDefaultLogCategory(ConfigurationManager.AppSettings["Fanex.Logging:DefaultCategory"])
                 .Use(GetLogEngines());

            var loggingConfiguration = new LoggingConfiguration()
                .HideSensitiveInfo(GetRegexToHideSensitiveInfo())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            ExceptionHandler.UseConfig(loggingConfiguration);
        }

        private static IEnumerable<ILogEngine> GetLogEngines()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var rabbitMQConnectionStrings = appSettings["Fanex.Logging:RabbitMQConnectionStrings"];
            var sentryUrl = appSettings["Fanex.Logging:SentryUrl"];

            if (!string.IsNullOrWhiteSpace(rabbitMQConnectionStrings))
            {
                yield return new RabbitMQLogging(rabbitMQConnectionStrings);
            }

            if (!string.IsNullOrWhiteSpace(sentryUrl))
            {
                yield return new SentryLogging(
                    new SentryEngineOptions
                    {
                        Dsn = new Dsn(sentryUrl)
                    },
                    sentryScript: $"{AppSettings.AssetAppPath}/bundles/common/raven.min.js");
            }
        }

        private static List<string> GetRegexToHideSensitiveInfo()
        {
            var result = new List<string>();
            var sensitiveInfo = new[] { "password", "seccode", "securitycode" };

            foreach (var info in sensitiveInfo)
            {
                result.Add($"(?:{info}=)(?<paramValue>[^&|^\\s]+)");
                result.Add($"(\"{info}\":)(?<paramValue>[^,|^\r]+)");
            }

            return result;
        }
    }
}