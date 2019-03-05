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

        public long GetOnlineMember(int viewerId)
            => repository.Get<int>(new GetTotalMemberOnlineCriteria { CustId = viewerId });

    }
}
