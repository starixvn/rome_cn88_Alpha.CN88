namespace Alpha.CN88.Site.Requests.Password
{
    using System.Collections.Generic;
    using CN88.Models;
    using global::CN88.Auth.Models;
    using MediatR;

    public class ChangePasswordCommand : BaseQuery, IAsyncRequest<SecurityResult>
    {
        public ChangePasswordCommand(IUser viewer)
            : base(viewer)
        {
            IsSubAccount = viewer.IsSubAcc;
            SubUserName = viewer.SubUserName;
            NickName = viewer.NickName;
            LanguageKey = viewer.LanguageKey;
        }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public bool IsSubAccount { get; }

        public string SubUserName { get; }

        public string NickName { get; }

        public string LanguageKey { get; }

        public IEnumerable<string> InvalidPasswords
        {
            get
            {
                yield return ViewerName;

                if (!string.IsNullOrWhiteSpace(NickName))
                {
                    yield return NickName;
                }

                if (IsSubAccount)
                {
                    yield return SubUserName;
                }
            }
        }
    }
}