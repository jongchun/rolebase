using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using UsersAndRolesDemo.Models;

namespace UsersAndRolesDemo.Services
{
    public class EmailService
    {
        private string apiKey = "API KEY";
        private string adminEmail = "ADMIN EMAIL";
        public void SendEmail(string email, string subject, string content)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(adminEmail, "From Sea-to-Sky Cabins"),
                Subject = subject,
                PlainTextContent = content,
                HtmlContent = content
            };
            msg.AddTo(new EmailAddress(email, "To Member"));
            client.SendEmailAsync(msg);
        }

        public void ContactUS(Contact contact)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(contact.Email, "From " + contact.Name),
                Subject = contact.Title,
                PlainTextContent = contact.Message,
                HtmlContent = contact.Message
            };
            msg.AddTo(new EmailAddress(adminEmail, "To Admin"));
            client.SendEmailAsync(msg);
        }
    }
}
