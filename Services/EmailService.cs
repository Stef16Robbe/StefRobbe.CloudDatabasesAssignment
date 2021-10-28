using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Helpers;
using Domain;
using Helpers;
using SendGrid.Helpers.Mail;

namespace Services
{
    public class EmailService : IEmailService
    {
        private readonly IBlobService _blobService;
        private readonly IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository, IBlobService blobService)
        {
            _emailRepository = emailRepository;
            _blobService = blobService;
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsers()
        {
            return await _emailRepository.GetAllUsers();
        }

        public async Task SendMails(IEnumerable<UserInfo> users)
        {
            foreach (var userInfo in users)
            {
                var pdfBA = await _blobService.GetBlobFromServer(userInfo.BlobId);
                var pdfB64 = ConvertToBase64(pdfBA);

                EmailUtil.SendMail(new EmailAddress(userInfo.Email), "Your mortgage stuff.", "Your mortgage stuff.",
                    "Your mortgage stuff.", pdfB64, "Mortgage");
            }
        }

        private static string ConvertToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}