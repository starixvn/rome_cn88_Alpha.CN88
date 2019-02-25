namespace Alpha.CN88.Criteria.Home
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class GetStatusSummaryCriteria : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new Dictionary<UserLevel, string>
        {
            [UserLevel.Agent] = "Age_CustomerSelStatisticsInfo_Ag",
            [UserLevel.Master] = "Age_CustomerSelStatisticsInfo_MAg",
            [UserLevel.Super] = "Age_CustomerSelStatisticsInfo_SAg"
        };

        public GetStatusSummaryCriteria(int custId, UserLevel userLevel)
        {
            CustId = custId;
            UserLevel = userLevel;
        }

        public int CustId { get; }

        [SpParam(Ignored = true)]
        public UserLevel UserLevel { get; }

        public override string GetSettingKey() => SettingKeys[UserLevel];

        public override bool IsValid() => true;
    }
}
