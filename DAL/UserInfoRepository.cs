using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly UserContext _userContext;

        public UserInfoRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task AddBlobId(UserInfo userInfo)
        {
            _userContext.Update(userInfo);
        }

        public async Task<UserInfo> CreateUserInfo(UserInfo userInfo)
        {
            _userContext.Users.Add(userInfo);
            await _userContext.SaveChangesAsync();
            return await _userContext.Users.FindAsync(userInfo.id);
        }

        public async Task UpdateUserInfo(UserInfo userInfo)
        {
            _userContext.Update(userInfo);
            await _userContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserInfo>> GetAllUserInfo()
        {
            return _userContext.Users.ToList();
        }

        public async Task<UserInfo> GetUserInfoById(string id)
        {
            return await _userContext.Users.FindAsync(id);
        }
    }
}