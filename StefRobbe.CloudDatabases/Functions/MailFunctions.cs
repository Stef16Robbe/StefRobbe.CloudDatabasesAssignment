using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Services;

namespace StefRobbe.CloudDatabases.Functions
{
    public class MailFunctions
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<MailFunctions> _logger;

        public MailFunctions(ILogger<MailFunctions> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        // [Function("SendMail")]
        // public async Task<HttpResponse> SendMail(
        //     [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        // {

        [Function("SendMail")]
        public async Task SendMail([TimerTrigger("0 * * * * *")] MyInfo myTimer, FunctionContext context)
        {
            _logger.LogInformation("C# Timer trigger function executed at: {DateTime}", DateTime.Now);

            // get all user emails
            var allUsers = await _emailService.GetAllUsers();
            // chunk them so we can start threads with less users in them
            var chunkedUsers = ChunkUtil.ChunkList(allUsers, 3);

            // chunked by N, so N threads are started that each send emails
            foreach (var userList in chunkedUsers)
            {
                var t = StartSendMailThread(userList);
            }
        }

        private Thread StartSendMailThread(IEnumerable<UserInfo> users)
        {
            var t = new Thread(() => _emailService.SendMails(users));
            t.Start();
            return t;
        }
    }
}