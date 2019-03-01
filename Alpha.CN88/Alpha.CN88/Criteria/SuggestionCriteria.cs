namespace Alpha.CN88.Criteria
{
    using Fanex.Data.Repository;

    public class SuggestionCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool IsInternalAgent { get; set; }

        public string Username { get; set; }

        public bool IsSubAccount { get; set; }

        public bool ExcludeMember { get; set; }

        public bool DirectDownline { get; set; }

        public bool IsAccInfo { get; set; }

        public override string GetSettingKey()
        {
            return "Age_CustomerSelUserSearch";
        }

        public override bool IsValid()
        {
            return CustId > 0 && !string.IsNullOrWhiteSpace(Username);
        }
    }
}