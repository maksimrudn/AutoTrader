using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Models.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure.Stock
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _mailSettings;

        public EmailService(IConfiguration configuration, IOptions<EmailSettings> mailSettings) {
            this._configuration = configuration;
            this._mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_configuration["ProductName"], _mailSettings.NotificationEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailSettings.SMTPServer, int.Parse(_mailSettings.SMTPPort) , false);
                await client.AuthenticateAsync(_mailSettings.NotificationEmail, _mailSettings.NotificationEmailPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
