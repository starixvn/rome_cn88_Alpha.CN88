namespace Alpha.CN88.Models
{
    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        string Language { get; }

        string LanguageKey { get; }

        byte LevelId { get; }

        bool IsInternal { get; }

        bool IsSubAcc { get; }

        int SubAccountId { get; }

        string SubUserName { get; }

        string IP { get; }

        bool HasUpdatePermission { get; }

        bool HasAddNewPermission { get; }

        int OnlineUserId { get; }

        string NickName { get; }

        int ServerId { get; }

        bool IsLogged { get; }

        bool IsPasswordExpired { get; }

        bool IsForcedSecurityCode { get; }

        string SecurityCode { get; }

        bool ClearClientData { get; set; }
    }
}