namespace Alpha.CN88.Site.Tests.TestModels
{
    using Alpha.CN88.Models;

    public class UserTest : IUser
    {
        public UserTest() : base()
        {
            Id = 1;
            Name = "user test";
            OnlineUserId = 2;
            NickName = "Test1";
            ServerId = 999;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Language { get; set; }

        public string LanguageKey { get; private set; }

        public byte LevelId { get; private set; }

        public bool IsInternal { get; set; }

        public bool IsSubAcc { get; private set; }

        public int SubAccountId { get; private set; }

        public string SubUserName { get; private set; }

        public string IP { get; private set; }

        public bool AccountManagementEditView { get; set; }

        public bool AccountManagementViewOnly { get; set; }

        public int OnlineUserId { get; private set; }

        public string NickName { get; set; }

        public int ServerId { get; private set; }

        public bool IsLogged { get; private set; }

        public bool IsPasswordExpired { get; private set; }

        public bool IsForcedSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public bool ClearClientData { get; set; }

        public byte CustomerStatus { get; set; }

        public string AdminName { get; private set; }

        public bool IsAllDownline { get; private set; }

        public bool IsHideTopMenu { get; }
    }
}