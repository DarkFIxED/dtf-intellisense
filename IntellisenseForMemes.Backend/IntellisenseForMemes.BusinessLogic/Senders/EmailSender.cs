using System.Threading.Tasks;
using IntellisenseForMemes.Common.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IntellisenseForMemes.BusinessLogic.Senders
{
    public class EmailSender : IEmailSender
    {
        private readonly SendGridClient _client;

        private readonly SendGridSettings _sendGridSettings;

        public EmailSender(SendGridSettings sendGridSettings)
        {
            _sendGridSettings = sendGridSettings;
            _client = new SendGridClient(sendGridSettings.ApiKey);
        }

        public async Task SendEmail(string recipientEmail, string recipientName, string subject, string htmlBody)
        {
            var from = new EmailAddress(_sendGridSettings.EmailSender, _sendGridSettings.NameSender);
            var to = new EmailAddress(recipientEmail, recipientName);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlBody);
            var response = await _client.SendEmailAsync(msg);
        }
    }
}
