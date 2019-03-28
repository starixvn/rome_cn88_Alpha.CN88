namespace Alpha.CN88.Site.Requests.Password
{
    using System.Collections.Generic;
    using CN88.Models;

    public class ResetDownlinePasswordCommand : ResetSubAccountPasswordCommand
    {
        public ResetDownlinePasswordCommand(IUser viewer)
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
            => base.IsValid() && viewer.HasUpdatePermission;
    }
}