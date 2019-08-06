namespace Alpha.CN88.Models.Home
{
    using Alpha.CN88.AppResources;
    using global::CN88.Web.Models;

    public class StatusSummary
    {
        public byte RoleId { get; }

        public UserLevel UserLevel => Enumeration.FromValue<UserLevel>(RoleId);

        public string Name => Resources.ResourceManager.GetString(UserLevel.DisplayName);

        public int Active { get; private set; }

        public int Inactive { get; private set; }

        public int ViewOnly { get; private set; }

        public int Suspended { get; private set; }

        public void Add(StatusSummary s)
        {
            Active += s.Active;
            Inactive += s.Inactive;
            ViewOnly += s.ViewOnly;
            Suspended += s.Suspended;
        }
    }
}
