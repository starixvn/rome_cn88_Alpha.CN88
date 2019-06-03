namespace CN88.Web.Services
{
    using System;
    using System.Linq;
    using Configurations;
    using Models;

    public interface AccountPermission
    {
        bool IsAuthorized(BelongingContext belongingContext, PageAccessElement pageAccessElement);
    }

    public class SubAccountPermission : AccountPermission
    {
        public bool IsAuthorized(BelongingContext belongingContext, PageAccessElement pageAccessElement)
            => pageAccessElement == null
                || pageAccessElement.Permission
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(item => belongingContext.SubPageAccess.GetPermission(item));
    }

    public class AccountStatusPermission : AccountPermission
    {
        private const byte Active = 4;

        public bool IsAuthorized(BelongingContext belongingContext, PageAccessElement pageAccessElement)
            => pageAccessElement == null
            || !pageAccessElement.IsDenyUpdate
            || !belongingContext.IsHttpPost
            || belongingContext.CustomerStatus == Active;
    }
}
