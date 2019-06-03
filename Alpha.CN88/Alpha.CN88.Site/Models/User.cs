namespace Alpha.CN88.Site.Models
{
    using CN88.Models;
    using global::CN88.Auth.Constants;
    using global::CN88.Auth.Services;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
        private readonly IUserSession userSession;

        public User(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Language => userSession.Language;

        public string LanguageKey => userSession.LangKey;

        public byte LevelId => userSession.RoleId;

        public bool IsInternal => userSession.IsInternal;

        public bool IsSubAcc => userSession.IsSubAcc;

        public int SubAccountId => userSession.SubAccId;

        public string SubUserName => userSession.SubUserName;

        public string IP => userSession.UserLoginIp;

        public bool AccountManagementEditView => userSession.SubPageAccess.UpdateAccount;

        public bool AccountManagementViewOnly => userSession.SubPageAccess.ViewMemberInfo;

        public int OnlineUserId => userSession.OnUserId;

        public string NickName => userSession.NickName;

        public int ServerId => userSession.ServerId;

        public bool IsLogged => userSession.IsLogged;

        public bool IsPasswordExpired => userSession.IsPasswordExpired;

        public bool IsForcedSecurityCode => userSession.IsForcedSecurityCode;

        public string SecurityCode => userSession.SecurityCode;

        public bool ClearClientData
        {
            get
            {
                return SessionHelper.Get<bool>(SessionKeys.ClearClientData);
            }

            set
            {
                SessionHelper.Add(SessionKeys.ClearClientData, value);
            }
        }

        public string AdminName => userSession.AdminName;
    }
}