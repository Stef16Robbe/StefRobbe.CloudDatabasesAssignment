using System;
using System.IO;
using Microsoft.Azure.Cosmos.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace StefRobbe.CloudDatabases.Helpers
{
    public static class EmailUtil
    {
        public static async void SendMail(EmailAddress to, string subject, string plainTextContent, string htmlContent, string b64Content = null, string attachmentName = null)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var email = Environment.GetEnvironmentVariable("BUYMYHOUSE_EMAIL");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(email, "Startup Bureau");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (!string.IsNullOrEmpty(b64Content) && !string.IsNullOrEmpty(attachmentName))
            {
                msg.AddAttachment(attachmentName, b64Content);
            }
            
            await client.SendEmailAsync(msg);
        }
    }
}