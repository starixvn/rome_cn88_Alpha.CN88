using System.Collections.Generic;

namespace Alpha.CN88.Constants
{
    public static class Language
    {
        public const string English = "en-US";
        public const string ChineseTraditional = "zh-TW";
        public const string ChineseSimplified = "zh-Hans";

        public const string Default = ChineseSimplified;

        public static readonly Dictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [ChineseSimplified] = "简体",
            [ChineseTraditional] = "繁體",
            [English] = "English"
        };

        public static readonly Dictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [ChineseSimplified] = "CN",
            [ChineseTraditional] = "CT",
            [English] = "E"
        };
    }
}