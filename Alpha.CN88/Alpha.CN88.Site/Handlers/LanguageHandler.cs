namespace Alpha.CN88.Site.Handlers
{
    using System;
    using System.Linq;
    using Constants;
    using MediatR;
    using Requests;
    using ViewModels;

    public class LanguageHandler :
        IRequestHandler<AllLanguagesQuery, LanguageViewModel>,
        IRequestHandler<ChangeLanguageCommand, string>
    {
        public LanguageViewModel Handle(AllLanguagesQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var viewModel = new LanguageViewModel(Language.CodeNames, Standardize(message.LanguageCode));

            return viewModel;
        }

        private static string Standardize(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode)
              || !Language.CodeNames.Keys.Contains(languageCode))
            {
                return Language.Default;
            }

            return languageCode;
        }

        public string Handle(ChangeLanguageCommand message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Command cannot be null");
            }

            var languageCode = Standardize(message.LanguageCode);

            message.ChangeLanguage(languageCode);

            return languageCode;
        }
    }
}