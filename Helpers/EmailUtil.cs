using System;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Helpers
{
    public class EmailUtil
    {
        private static ILogger<EmailUtil> _logger;

        public EmailUtil(ILogger<EmailUtil> logger)
        {
            _logger = logger;
        }


        public static async void SendMail(EmailAddress to, string subject, string plainTextContent, string htmlContent,
            string b64Content = null, string attachmentName = null)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var email = Environment.GetEnvironmentVariable("STARTUPDESK_EMAIL");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(email, "Startup Bureau");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (!string.IsNullOrEmpty(b64Content) && !string.IsNullOrEmpty(attachmentName))
                msg.AddAttachment(attachmentName, b64Content, "application/pdf");

            await client.SendEmailAsync(msg);

            _logger.LogInformation("Email sent successfully to: {ToEmail}", to.Email);
        }
    }
}