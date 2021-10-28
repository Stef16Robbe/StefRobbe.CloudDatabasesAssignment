using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public interface IEmailRepository
    {
        Task<IEnumerable<UserInfo>> GetAllUsers();
    }
}