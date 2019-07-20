using Alpha.CN88.Criteria;
using Fanex.Data.Repository;

namespace Alpha.CN88.Services.Impl
{
    public class DefaultServiceImpl : DefaultService
    {
        private readonly IDynamicRepository repository;

        public DefaultServiceImpl(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public long GetOnlineMember(int viewerId, int subAccId, bool isAllDownline)
            => repository.Get<int>(new GetTotalMemberOnlineCriteria(viewerId, subAccId, isAllDownline));
    }
}