namespace Alpha.CN88.Site.Requests.Password
{
    using System.Collections.Generic;
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class ResetSubAccountPasswordCommand : BaseQuery, IRequest<SecurityResult>
    {
        public ResetSubAccountPasswordCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string NewPassword { get; set; }

        public string LanguageKey { get; }

        public virtual IEnumerable<string> InvalidPasswords
        {
            get
            {
                yield return CustName;
            }
        }

        public IUser GetViewer()
            => viewer;

        public override bool IsValid()
            => base.IsValid()
            && CustId != ViewerId;
    }
}