namespace Alpha.CN88.Models
{
    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        string Language { get; }

        byte LevelId { get; }

        bool IsInternal { get; }

        int SubAccountId { get; }

        string IP { get; }

        bool HasUpdatePermission { get; }

        bool HasAddNewPermission { get; }

        int OnlineUserId { get; }

        string SubUserName { get; }

        string NickName { get; }

        int ServerId { get; }
    }
}