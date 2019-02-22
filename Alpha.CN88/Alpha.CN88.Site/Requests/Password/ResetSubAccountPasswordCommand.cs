namespace Alpha.CN88.Site.Requests.Password
{
    using System.Collections.Generic;
    using global::CN88.Auth.Models;
    using MediatR;

    public class ResetSubAccountPasswordCommand : BaseQuery, IAsyncRequest<Result>
    {
        public ResetSubAccountPasswordCommand(CN88.Models.IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string LanguageKey { get; }

        public virtual IEnumerable<string> InvalidPasswords
        {
            get
            {
                yield return CustName;
            }
        }

        public override bool IsValid()
            => base.IsValid()
            && CustId != ViewerId;
    }
}