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
                ActorName = userSession.UserName;
                ActorRoleId = userSession.RoleId;
                SubAccountId = userSession.SubAccId;
                SubUserName = userSession.SubUserName;
                SubPageAccess = userSession.SubPageAccess;
                OnlineUserId = userSession.OnUserId;
                CurrentUrl = currentUrl ?? GetUrlFromHttpRequest();
                CustomerStatus = userSession.GetProperty<byte>(CustomerStatusSessionKey);
                IsHttpPost = HttpRequest.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase);
                ServerId = userSession.ServerId;
                NickName = userSession.NickName;
            }
        }

        public HttpRequestBase HttpRequest { get; }

        public bool IsValid { get; }

        public int ActorId { get; }

        public string ActorName { get; }

        public byte ActorRoleId { get; }

        public int SubAccountId { get; }

        public string SubUserName { get; }

        public int OnlineUserId { get; }

        public PageAccess SubPageAccess { get; }

        public byte CustomerStatus { get; }

        public bool IsHttpPost { get; }

        public int ServerId { get; }

        public string NickName { get; }

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
