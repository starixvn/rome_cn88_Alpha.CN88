using System.Data;
using System.Threading.Tasks;

namespace Alpha.CN88.Services
{
    public interface SuggestionService
    {
        Task<DataSet> SearchUser(int userId, string query, bool issubaccount, bool excludemember, bool directdownline, bool isaccinfo, bool isInternal);
    }
}