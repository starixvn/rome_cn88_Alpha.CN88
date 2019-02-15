namespace Alpha.CN88.Site.ViewModels
{
    using System.Collections.Generic;
    using Constants;

    public class LanguageViewModel
    {
        public LanguageViewModel(
            Dictionary<string, string> data,
            string currentLanguageCode = Language.Default)
        {
            Data = data ?? Language.CodeNames;
            CurrentLanguageCode = currentLanguageCode;

            CurrentLanguageName = Language.CodeNames[CurrentLanguageCode];
        }

        public Dictionary<string, string> Data { get; }

        public string CurrentLanguageCode { get; }

        public string CurrentLanguageName { get; }
    }
}