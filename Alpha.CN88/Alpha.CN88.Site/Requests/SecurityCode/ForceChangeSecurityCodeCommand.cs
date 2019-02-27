namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;

    public class ForceChangeSecurityCodeCommand : ChangeSecurityCodeFirstLoginCommand
    {
        public ForceChangeSecurityCodeCommand(IUser viewer)
            : base(viewer)
        {
        }

        public string OldSecurityCode { get; set; }
    }
}