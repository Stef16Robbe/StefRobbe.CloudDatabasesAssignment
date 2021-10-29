using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IUserInfoService
    {
        Task<UserInfo> CreateUserInfo(UserInfo userInfo);
        Task<IEnumerable<UserInfo>> GetAllUserInfo();
        Task<UserInfo> GetUserInfoById(string id);
        Task CreateMortgageApplication(string queueMessage);
        Task DeleteMortgage(string userInfoId);
        Task DeleteBlobId();
    }
}