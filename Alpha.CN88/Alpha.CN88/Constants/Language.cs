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
            [English] = "English",
            [ChineseTraditional] = "繁體",
            [ChineseSimplified] = "简体"
        };

        public static readonly Dictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [English] = "E",
            [ChineseTraditional] = "CT",
            [ChineseSimplified] = "CN"
        };
    }
}