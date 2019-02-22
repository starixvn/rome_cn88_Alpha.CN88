namespace Alpha.CN88.Site.ViewModels.SecurityCode
{
    using CN88.Models;

    public class SecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string HashKey { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel
            => Enumeration.FromValue<UserLevel>(CustRoleId).DisplayName.ToLower();

        public string Title { get; set; }
    }
}