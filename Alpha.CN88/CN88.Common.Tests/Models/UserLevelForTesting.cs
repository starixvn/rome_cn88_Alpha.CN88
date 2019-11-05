using CN88.Common.Models;

namespace CN88.Common.Tests.Models
{
    public class UserLevelForTesting : UserLevel
    {
        public static readonly new UserLevelForTesting Agent = new UserLevelForTesting(1, "Super");

        public UserLevelForTesting()
        {
        }

        private UserLevelForTesting(byte value, string displayName)
        {
        }
    }
}