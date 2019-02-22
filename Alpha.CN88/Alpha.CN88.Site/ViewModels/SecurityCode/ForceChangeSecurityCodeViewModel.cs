namespace Alpha.CN88.Site.ViewModels.SecurityCode
{
    public class ForceChangeSecurityCodeViewModel : SecurityCodeViewModel
    {
        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public string Username { get; set; }

        public string AlternateUsername { get; set; }

        public string UsernameShower { get; set; }
    }
}