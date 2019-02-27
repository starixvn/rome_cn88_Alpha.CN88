namespace Alpha.CN88.Site.Requests.SecurityCode
{
    using CN88.Models;

    public class IndexQuery : BaseQuery
    {
        public IndexQuery(IUser viewer)
            : base(viewer)
        {
            IsForcedSecurityCode = viewer.IsForcedSecurityCode;
            SecurityCode = viewer.SecurityCode;
        }

        public bool IsForcedSecurityCode { get; }

        public string SecurityCode { get; }
    }
}