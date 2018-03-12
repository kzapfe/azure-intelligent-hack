using System;
using SendGrid.Helpers.Mail;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using SendGrid;
using System.Threading.Tasks;
using IntelligentHack.Domain;

namespace IntelligentHack.Functions.Classes
{
    public class MailManager
    {
        public static async Task RegistrationMail(string mailTo, string title, string details, Person person)
        {
            var client = new SendGridClient(Settings.SendGridAPIKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("registration@intelligenthack.com", "Intelligent Hack Registration Mail Bot"),
                Subject = "Intelligent Hack Registration Details",
                HtmlContent = $"<strong>{title}</strong>" +
                $"<br />{details}<br/><br/>" +
                $"<strong>Additional Information (Name Complete): </strong>{person.Name + " " + person.Lastname}"
            };
            msg.AddTo(new EmailAddress(mailTo));
            var response = await client.SendEmailAsync(msg);
        }
    }
}