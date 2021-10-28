using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public class EmailRepository : IEmailRepository
    {
        private readonly UserContext _uContext;

        public EmailRepository(UserContext uContext)
        {
            _uContext = uContext;
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsers()
        {
            var users = _uContext.Users.ToList();
            return users;
        }
    }
}