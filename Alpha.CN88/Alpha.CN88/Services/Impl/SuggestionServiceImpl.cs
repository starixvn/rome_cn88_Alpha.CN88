namespace Alpha.CN88.Services.Impl
{
    using Alpha.CN88.Models;
    using Criteria;
    using Fanex.Data.Repository;
    using Params;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SuggestionServiceImpl : SuggestionService
    {
        private readonly IDynamicRepository repository;

        public SuggestionServiceImpl(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<SuggestionModel>> SearchUser(SuggestionParam param)
        {
            var criteria = new SuggestionCriteria
            {
                CustId = param.CustId,
                DirectDownline = param.DirectDownline,
                ExcludeMember = param.ExcludeMember,
                IsAccInfo = param.IsAccInfo,
                IsInternalAgent = param.IsInternal,
                IsSubAccount = param.IsSubAccount,
                Username = param.Username
            };

            return repository.FetchAsync<SuggestionModel>(criteria);
        }
    }
}