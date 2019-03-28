namespace CN88.SubAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [Serializable]
    public class PageAccess : ISerializable
    {
        private readonly IDictionary<string, bool> permissions;

        public PageAccess()
        {
            permissions = new Dictionary<string, bool>(PageAccessType.AccessTypes);
        }

        public PageAccess(SerializationInfo info, StreamingContext context)
            : this()
        {
            foreach (var key in permissions.Keys)
            {
                permissions[key] = info.GetBoolean(key);
            }
        }

        public bool TotalBets
        {
            get
            {
                return permissions[PageAccessType.TotalBets];
            }

            set
            {
                permissions[PageAccessType.TotalBets] = value;
            }
        }

        public bool AccountManagementViewOnly
        {
            get
            {
                return permissions[PageAccessType.AccountManagementViewOnly];
            }

            set
            {
                permissions[PageAccessType.AccountManagementViewOnly] = value;
            }
        }

        public bool AccountManagementEditView
        {
            get
            {
                return permissions[PageAccessType.AccountManagementEditView];
            }

            set
            {
                permissions[PageAccessType.AccountManagementEditView] = value;
            }
        }

        public bool Reports
        {
            get
            {
                return permissions[PageAccessType.Reports];
            }

            set
            {
                permissions[PageAccessType.Reports] = value;
            }
        }

        public void SetPermission(string key, bool value)
        {
            if (permissions.ContainsKey(key))
            {
                permissions[key] = value;
            }
        }

        public bool GetPermission(string key)
        {
            bool value;
            permissions.TryGetValue(key, out value);

            return value;
        }

        public void SetFullPermission()
        {
            foreach (var key in permissions.Keys)
            {
                permissions[key] = true;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var key in permissions.Keys)
            {
                info.AddValue(key, permissions[key]);
            }
        }

        public static implicit operator string(PageAccess value)
        {
            var valuePermissions = value.permissions;

            return string.Join(string.Empty, valuePermissions.Select(kv => kv.Value ? kv.Key : string.Empty));
        }

        public static implicit operator PageAccess(string value)
        {
            var pageAccess = new PageAccess();

            if (!string.IsNullOrWhiteSpace(value))
            {
                var permissions = pageAccess.permissions;

                foreach (var key in permissions.Keys)
                {
                    permissions[key] = value.Contains(key);
                }
            }

            return pageAccess;
        }
    }
}
