namespace Alpha.CN88.Site.Requests.Password
{
    using System.Collections.Generic;

    public class ResetMainAccountPasswordCommand : ResetSubAccountPasswordCommand
    {
        public ResetMainAccountPasswordCommand(CN88.Models.IUser viewer)
            : base(viewer)
        {
        }

        public override IEnumerable<string> InvalidPasswords
        {
            get
            {
                yield return ViewerName;
            }
        }

        public override bool IsValid()
            => base.IsValid()
            && CustId != ViewerId;
    }
}