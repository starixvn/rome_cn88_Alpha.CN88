namespace Alpha.CN88.Site.Handlers
{
    using System;
    using global::CN88.Web.Helpers;
    using global::CN88.Web.Models;
    using global::CN88.Web.ViewModel;
    using MediatR;
    using Requests;
    using Requests.Default;
    using Services;
    using Sunplus.Agent.Utilities;
    using ViewModels.Default;

    public class DefaultHandler :
        IRequestHandler<BrowserQuery, BrowserViewModel>,
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<TotalOnlineMember, long>
    {
        private const string SawBrowserWarningPageSessionKey = "DefaultHandler.SawBrowserWarningPageSessionKey";

        private readonly SessionManager sessionManager;
        private readonly BrowserHelper browserHelper;
        private readonly DefaultService defaultService;
        private readonly Func<string> getDefaultUrl;
        private readonly Func<string, string> encodeDataFunction;

        public DefaultHandler(
            SessionManager sessionManager,
            BrowserHelper browserHelper,
            DefaultService defaultService)
            : this(sessionManager, browserHelper, defaultService, null, null)
        {
        }

        internal DefaultHandler(
            SessionManager sessionManager,
            BrowserHelper browserHelper,
            DefaultService defaultService,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
            this.defaultService = defaultService;
            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.encodeDataFunction = encodeDataFunction ?? Encryption.PearsonEncode;
        }

        public BrowserViewModel Handle(BrowserQuery message)
        {
            if (sessionManager.Get<bool>(SawBrowserWarningPageSessionKey))
            {
                return new BrowserViewModel();
            }

            var browser = browserHelper.CheckSupportedBrowsers();
            browser.UserLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);

            sessionManager.Add(SawBrowserWarningPageSessionKey, true);

            return browser;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var onlineMember = defaultService.GetOnlineMember(message.ViewerId, message.SubAccountId, message.IsAllDownline);

            return new IndexViewModel(message, browserHelper, onlineMember, getDefaultUrl, encodeDataFunction);
        }

        public long Handle(TotalOnlineMember message)
            => defaultService.GetOnlineMember(message.ViewerId, message.SubAccountId, message.IsAllDownline);
    }
}
