using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.Cosmos;

namespace DAL
{
    public interface IEmailRepository
    {
        Task<IEnumerable<UserInfo>> GetAllUsers();
    }
}