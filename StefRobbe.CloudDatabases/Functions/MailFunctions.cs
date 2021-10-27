using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using Services;
using StefRobbe.CloudDatabases.Helpers;

namespace StefRobbe.CloudDatabases
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

        [Function("SendMail")]
        public async Task SendMail([TimerTrigger("0 */5 * * * *")] MyInfo myTimer, FunctionContext context)
        {
            _logger.LogInformation("C# Timer trigger function executed at: {DateTime}", DateTime.Now);
            _logger.LogInformation("Next timer schedule at: {NextScheduled}", myTimer.ScheduleStatus.Next);

            // get all user emails
            var users = await _emailService.GetAllUsers();
            // get their corresponding PDF's
            foreach (var userInfo in users)
                // get specific PDF based on BlobId...

                // send emails
                EmailUtil.SendMail(new EmailAddress(userInfo.Email), "Your mortgage stuff.", "Your mortgage stuff.",
                    "Your mortgage stuff.", "", "");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}