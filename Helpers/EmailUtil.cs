using System;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Helpers
{
    public class EmailUtil
    {
        public static async void SendMail(EmailAddress to, string subject, string plainTextContent, string htmlContent)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var email = Environment.GetEnvironmentVariable("STARTUPDESK_EMAIL");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(email, "Startup Bureau");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);

            Console.WriteLine("Email sent successfully to: " + to.Email);
        }
    }
}