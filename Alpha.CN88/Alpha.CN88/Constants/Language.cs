using System.Collections.Generic;

namespace Alpha.CN88.Constants
{
    public static class Language
    {
        public const string English = "en-US";
        public const string ChineseTraditional = "zh-TW";
        public const string ChineseSimplified = "zh-CN";
        public const string NewChineseSimplified = "zh-Hans";
        public const string Japanese = "ja-JP";
        public const string Thai = "th-TH";
        public const string Korean = "ko-KR";

        public const string Default = English;

        public static readonly Dictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            [ChineseTraditional] = "繁體中文",
            [ChineseSimplified] = "简体中文",
            [NewChineseSimplified] = "简体中文(新)",
            [Japanese] = "日本語",
            [Thai] = "ภาษาไทย",
            [Korean] = "한국어"
        };

        public static readonly Dictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [English] = "E",
            [ChineseTraditional] = "CT",
            [ChineseSimplified] = "CS",
            [NewChineseSimplified] = "CN",
            [Japanese] = "J",
            [Thai] = "TH",
            [Korean] = "KO"
        };
    }
}
