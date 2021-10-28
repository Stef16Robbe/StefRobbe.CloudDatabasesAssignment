using System;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Helpers
{
    public static class EmailUtil
    {
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
        }
    }
}