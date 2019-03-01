using Fanex.Data;
using System.Collections;
using System.Data;
using System.Threading.Tasks;

namespace Alpha.CN88.Services.Impl
{
    public class SuggestionServiceImpl : SuggestionService
    {
        public async Task<DataSet> SearchUser(int userId, string query, bool issubaccount, bool excludemember, bool directdownline, bool isaccinfo, bool isInternal)
        {
            using (var objectDb = ObjectDbFactory.CreateInstance("Age_CustomerSelUserSearch"))
            {
                var param = new Hashtable
                {
                    {"custid", userId},
                    {"username", query},
                    {"issubaccount", issubaccount},
                    {"excludemember", excludemember},
                    {"directdownline", directdownline},
                    {"isaccinfo", isaccinfo},
                    {"isinternalagent", isInternal}
                };

                objectDb.AssignParamValues(param);

                return await objectDb.ExecuteDataSetAsync();
            }
        }
    }
}