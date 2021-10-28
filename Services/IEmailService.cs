using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IEmailService
    {
        Task<IEnumerable<UserInfo>> GetAllUsers();
        IEnumerable<IEnumerable<UserInfo>> ChunkUsers(IEnumerable<UserInfo> users, int chunkSize);
        Task SendMails(IEnumerable<UserInfo> users);
    }
}