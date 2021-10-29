using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using DAL.Helpers;
using Domain;
using Helpers;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly IBlobService _blobService;
        private readonly IEmailRepository _emailRepository;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IEmailRepository emailRepository, IBlobService blobService, ILogger<EmailService> logger)
        {
            _emailRepository = emailRepository;
            _blobService = blobService;
            _logger = logger;
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsers()
        {
            return await _emailRepository.GetAllUsers();
        }

        public async Task SendMails(IEnumerable<UserInfo> users)
        {
            foreach (var userInfo in users)
            {
                _logger.LogInformation("Sending email to: {UserFirstName}", userInfo.FirstName);
                var pdf = await _blobService.GetBlobFromServer(userInfo.BlobId);

                EmailUtil.SendMail(new EmailAddress(userInfo.Email), "Your mortgage stuff.", "Your mortgage stuff.\n" + pdf,
                    "Your mortgage stuff.\n" + pdf);
            }
        }
    }
}