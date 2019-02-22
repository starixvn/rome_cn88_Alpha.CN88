namespace CN88.Auth.Params
{
    public class ChangeSecurityCodeParam : ResetSecurityCodeParam
    {
        public bool MustVerifyCurrentSecurityCode { get; set; }

        public string CurrentSecurityCode { get; set; }

        public string Salt { get; set; }
    }
}
