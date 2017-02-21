using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace UsersAndRolesDemo.Services
{
    public class EmailService
    {
        public void SendEmail(string email, string subject, string content)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("admin@example.com", "From Sea-to-Sky Cabins"),
                Subject = subject,
                PlainTextContent = content,
                HtmlContent = content
            };
            msg.AddTo(new EmailAddress(email, "To Member"));
            client.SendEmailAsync(msg);
        }
    }
}