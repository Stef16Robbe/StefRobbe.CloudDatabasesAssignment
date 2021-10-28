using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IEmailService
    {
        Task<IEnumerable<UserInfo>> GetAllUsers();
        Task SendMails(IEnumerable<UserInfo> users);
    }
}