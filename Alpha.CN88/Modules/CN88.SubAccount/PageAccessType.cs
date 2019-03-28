namespace CN88.SubAccount
{
    using System.Collections.Generic;

    public static class PageAccessType
    {
        public const string TotalBets = "[A]";
        public const string AccountManagementViewOnly = "[B]";
        public const string AccountManagementEditView = "[C]";
        public const string Reports = "[D]";

        private static IDictionary<string, bool> accessTypes = new Dictionary<string, bool>
        {
            [TotalBets] = false,
            [AccountManagementViewOnly] = false,
            [AccountManagementEditView] = false,
            [Reports] = false
        };

        public static IDictionary<string, bool> AccessTypes
            => accessTypes;

        public static bool IsValidType(string type)
            => accessTypes.ContainsKey(type);
    }
}
