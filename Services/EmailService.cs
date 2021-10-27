using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Domain;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }
        
        public async Task<IEnumerable<UserInfo>> GetAllUsers()
        {
            return await _emailRepository.GetAllUsers();
        }
    }
}