namespace CN88.Auth.Services
{
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;

    public interface AuthenticationService
    {
        Task LogoutAsync(
           int onlineUserId,
           string userName,
           byte roleId,
           int serverId,
           string nickName);
    }

    public class AuthenticationServiceImpl : AuthenticationService
    {
        private readonly IDynamicRepository dynamicRepository;

        public AuthenticationServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task LogoutAsync(
            int onlineUserId,
            string userName,
            byte roleId,
            int serverId,
            string nickName)
            => await dynamicRepository.ExecuteAsync(
                new LogoutCriteria(onlineUserId, userName, roleId, serverId, nickName));
    }
}
