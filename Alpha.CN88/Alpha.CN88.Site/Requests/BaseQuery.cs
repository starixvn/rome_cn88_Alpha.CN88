namespace Alpha.CN88.Site.Requests
{
    using CN88.Models;

    public class BaseQuery
    {
        protected readonly IUser viewer;

        protected BaseQuery(IUser viewer)
        {
            this.viewer = viewer;

            // Default data, if query string had data, then get from query string.
            CustId = viewer.Id;
            CustName = viewer.Name;
            CustLevelId = viewer.LevelId;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public int ViewerId => viewer.Id;

        public string ViewerName => viewer.Name;

        public byte ViewerLevelId => viewer.LevelId;

        public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public int SubAccountId => viewer.SubAccountId;

        public string IP => viewer.IP;

        public string Language => viewer.Language;

        public virtual bool IsValid()
            => CustId > 0
            && !string.IsNullOrWhiteSpace(CustName)
            && ActorLevel.IsEqualOrUplineOf(CustLevel)
            && !string.IsNullOrWhiteSpace(Language);
    }
}