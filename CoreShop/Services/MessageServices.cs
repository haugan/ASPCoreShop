using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Services
{
    // TWO-FACTOR AUTHENTICATION https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public AuthMessageSenderOptions Options { get; } // contains GridSender data in secrets.json

        public AuthMessageSender(IOptions<AuthMessageSenderOptions> options)
        {
            Options = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            SendGridMail(Options.SendGridKey, email, subject, message).Wait();
            return Task.FromResult(0);
        }

        public async Task SendGridMail(string apiKey, string email, string subject, string message)
        {
            var gridClient = new SendGridClient(apiKey);

            var gridMessage = new SendGridMessage()
            {
                From = new EmailAddress("admin@coreshop.net", "CoreSHOP Verification Admin"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            gridMessage.AddTo(new EmailAddress(email));
            var response = await gridClient.SendEmailAsync(gridMessage);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // SMS SERVICE GOES HERE
            return Task.FromResult(0);
        }
    }
}
