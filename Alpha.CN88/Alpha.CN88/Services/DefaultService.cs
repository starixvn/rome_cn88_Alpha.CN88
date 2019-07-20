namespace Alpha.CN88.Services
{
    public interface DefaultService
    {
        long GetOnlineMember(int viewerId, int subAccId, bool isAllDownline);
    }
}