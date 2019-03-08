namespace CN88.Web.Models
{
    public class BelongingInfo
    {
        public BelongingInfo(bool isActive, byte roleId)
        {
            IsActive = isActive;
            RoleId = roleId;
        }

        public bool IsActive { get; }

        public byte RoleId { get; }

        public bool IsValid()
            => IsActive && 0 < RoleId && RoleId < 5;
    }
}
