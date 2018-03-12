﻿using System;
using SendGrid.Helpers.Mail;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using SendGrid;
using System.Threading.Tasks;

namespace IntelligentHack.Functions.Classes
{
    public class MailManager
    {
        public static async Task RegistrationMail(string mailTo, string title, string details)
        {
            var client = new SendGridClient(Settings.SendGridAPIKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("registration@intelligenthack.com", "Intelligent Hack Registration Mail Bot"),
                Subject = "Intelligent Hack Registration Details",
                HtmlContent = $"<strong>{title}</strong><br />{details}"
            };
            msg.AddTo(new EmailAddress(mailTo));
            var response = await client.SendEmailAsync(msg);
        }
    }
}