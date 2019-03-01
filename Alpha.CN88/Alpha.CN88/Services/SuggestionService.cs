namespace Alpha.CN88.Services
{
    using Alpha.CN88.Models;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface SuggestionService
    {
        Task<IEnumerable<SuggestionModel>> SearchUser(ICriteria criteria);
    }
}