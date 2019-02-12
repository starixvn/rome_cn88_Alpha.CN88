namespace Alpha.CN88.Site.Requests
{
    using MediatR;
    using Sunplus.Agent.Model.Base;
    using ViewModels;

    public class AllLanguagesQuery : IRequest<LanguageViewModel>
    {
        private readonly string languageCode;

        public AllLanguagesQuery(string languageCode)
        {
            this.languageCode = languageCode ?? new UserSessionExt().Language;
        }

        public string LanguageCode
        {
            get
            {
                return languageCode;
            }
        }
    }
}