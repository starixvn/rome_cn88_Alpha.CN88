namespace Alpha.CN88.Site.Handlers
{
    using System;
    using Helpers;
    using MediatR;
    using Requests.Default;
    using Services;
    using Sunplus.Agent.Utilities;
    using ViewModels.Default;

    public class DefaultHandler :
        IRequestHandler<IndexQuery, IndexViewModel>
    {
        private readonly BrowserHelper browserHelper;
        private readonly DefaultService defaultService;
        private readonly Func<string> getDefaultUrl;
        private readonly Func<string, string> encodeDataFunction;

        public DefaultHandler(
            BrowserHelper browserHelper,
            DefaultService defaultService)
            : this(browserHelper, defaultService, null, null)
        {
        }

        internal DefaultHandler(
            BrowserHelper browserHelper,
            DefaultService defaultService,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction)
        {
            this.browserHelper = browserHelper;
            this.defaultService = defaultService;
            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.encodeDataFunction = encodeDataFunction ?? Encryption.PearsonEncode;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var onlineMember = defaultService.GetOnlineMember();

            return new IndexViewModel(message, browserHelper, onlineMember, getDefaultUrl, encodeDataFunction);
        }
    }
}