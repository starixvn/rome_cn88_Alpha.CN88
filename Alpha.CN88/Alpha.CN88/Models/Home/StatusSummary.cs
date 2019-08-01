namespace Alpha.CN88.Models.Home
{
    using Alpha.CN88.AppResources;
    using global::CN88.Web.Models;

    public class StatusSummary
    {
        public byte RoleId { get; }

        public string Name
        {
            get
            {
                var userLevel = Enumeration.FromValue<UserLevel>(RoleId);

                if (userLevel.IsSuperLevel())
                {
                    return Resources.Super;
                }

                if (userLevel.IsMasterLevel())
                {
                    return Resources.MasterAgent;
                }

                if (userLevel.IsAgentLevel())
                {
                    return Resources.Agent;
                }

                if (userLevel.IsMemberLevel())
                {
                    return Resources.Member;
                }

                return userLevel.IsHeadlineLevel()
                    ? Resources.Headline
                    : "Unknown";
            }
        }

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