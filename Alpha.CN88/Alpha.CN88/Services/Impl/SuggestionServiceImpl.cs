namespace Alpha.CN88.Services.Impl
{
    using Alpha.CN88.Models;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SuggestionServiceImpl : SuggestionService
    {
        private readonly IDynamicRepository repository;

        public SuggestionServiceImpl(IDynamicRepository repository)
        {
            this.repository = repository;    
        }

        public async Task<IEnumerable<SuggestionModel>> SearchUser(ICriteria criteria)
            => await repository.FetchAsync<SuggestionModel>(criteria);
    }
}