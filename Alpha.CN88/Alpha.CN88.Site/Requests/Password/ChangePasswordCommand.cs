namespace Alpha.CN88.Site.Requests.Password
{
    using System.Collections.Generic;
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class ChangePasswordCommand : BaseQuery, IRequest<SecurityResult>
    {
        public ChangePasswordCommand(IUser viewer)
            : base(viewer)
        {
            NickName = viewer.NickName;
            IsSubAccount = viewer.IsSubAcc;
            SubUserName = viewer.SubUserName;
            LanguageKey = viewer.LanguageKey;
        }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string NickName { get; }

        public bool IsSubAccount { get; }

        public string SubUserName { get; }

        public string LanguageKey { get; }

        public IEnumerable<string> InvalidPasswords
        {
            get
            {
                yield return ViewerName;
                yield return NickName;

                if (IsSubAccount)
                {
                    yield return SubUserName;
                }
            }
        }
    }
}