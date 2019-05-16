namespace CN88.Web.Configurations
{
    using System.Collections.Generic;
    using System.Linq;

    public static class DeniedPageHelper
    {
        private const int AgentRoleId = 2;
        private const int MasterRoleId = 3;
        private const int SuperRoleId = 4;

        private static IDictionary<int, IEnumerable<DeniedPageElement>> pages;

        public static IEnumerable<DeniedPageElement> GetDeniedPages(int roleId)
        {
            if (pages == null)
            {
                pages = new Dictionary<int, IEnumerable<DeniedPageElement>>
                {
                    [AgentRoleId] = DeniedPageForRoleId(AgentRoleId),
                    [MasterRoleId] = DeniedPageForRoleId(MasterRoleId),
                    [SuperRoleId] = DeniedPageForRoleId(SuperRoleId)
                };
            }

            IEnumerable<DeniedPageElement> deniedPageElements;

            if (!pages.TryGetValue(roleId, out deniedPageElements))
            {
                deniedPageElements= Enumerable.Empty<DeniedPageElement>();
            }

            return deniedPageElements;
        }

        private static IEnumerable<DeniedPageElement> DeniedPageForRoleId(int roleId)
        {
            var level = AuthorizationSection.Instance.GroupPageAccessList.FirstOrDefault(i => i.RoleId == roleId);

            return level != null
                ? level.DeniedPages.Cast<DeniedPageElement>()
                : Enumerable.Empty<DeniedPageElement>();
        }
    }
}
