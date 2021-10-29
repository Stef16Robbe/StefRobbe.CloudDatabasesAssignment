using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> CreateUserInfo(UserInfo userInfo);
        Task<IEnumerable<UserInfo>> GetAllUserInfo();
        Task<UserInfo> GetUserInfoById(string id);
        Task AddBlobId(UserInfo userInfo);
    }
}