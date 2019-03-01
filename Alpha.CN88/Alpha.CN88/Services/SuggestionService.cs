namespace Alpha.CN88.Services
{
    using Alpha.CN88.Models;
    using Params;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface SuggestionService
    {
        Task<IEnumerable<SuggestionModel>> SearchUser(SuggestionParam param);
    }
}