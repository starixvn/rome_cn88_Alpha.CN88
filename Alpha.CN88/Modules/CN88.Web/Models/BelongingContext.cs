namespace CN88.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Configurations;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public class BelongingContext
    {
        private const string CustomerStatusSessionKey = "CUSTOMERSTATUS";

        private readonly AuthorizationSection authorizationSection;

        public BelongingContext(
            HttpRequestBase httpRequest = null,
            AuthorizationSection authorizationSection = null,
            string currentUrl = null,
            IUserSession userSession = null)
        {
            HttpRequest = httpRequest ?? new HttpRequestWrapper(HttpContext.Current.Request);
            this.authorizationSection = authorizationSection ?? AuthorizationSection.Instance;

            userSession = userSession ?? new UserSessionExt();
            IsValid = userSession.UserId > 0 && !userSession.IsInternal;

            if (IsValid)
            {
                ActorId = userSession.CustId;
                ActorRoleId = userSession.RoleId;
                SubAccountId = userSession.SubAccId;
                SubPageAccess = userSession.SubPageAccess;
                OnlineUserId = userSession.OnUserId;
                CurrentUrl = currentUrl ?? GetUrlFromHttpRequest();
                CustomerStatus = userSession.GetProperty<int>(CustomerStatusSessionKey);
            }
        }

        public HttpRequestBase HttpRequest { get; }

        public bool IsValid { get; }

        public int ActorId { get; }

        public byte ActorRoleId { get; }

        public int SubAccountId { get; }

        public int OnlineUserId { get; }

        public PageAccess SubPageAccess { get; }

        public int CustomerStatus { get; }

        public bool EnableCheckBelonging
            => authorizationSection.EnableCheckBelonging;

        public bool EnableKickout
            => !string.IsNullOrWhiteSpace(authorizationSection.KickoutUrl);

        public IEnumerable<GroupPageAccessElement> GroupPageAccessList
            => authorizationSection.GroupPageAccessList;

        public string[] CustIdQueryStringParamNames
            => authorizationSection.CustIdQueryStringParamNames;

        public string[] CustIdJsonParamNames
            => authorizationSection.CustIdJsonParamNames;

        public int CachedExpireTime
            => authorizationSection.CachedExpireTime;

        public string CurrentUrl { get; }

        public bool IsCurrentUrlContains(string url)
            => CurrentUrl.IndexOf(url, StringComparison.OrdinalIgnoreCase) >= 0;

        private string GetUrlFromHttpRequest()
            => HttpRequest.AppRelativeCurrentExecutionFilePath.Substring(2);
    }
}
