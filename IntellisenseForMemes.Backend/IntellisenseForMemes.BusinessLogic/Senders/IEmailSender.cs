using System.Threading.Tasks;

namespace IntellisenseForMemes.BusinessLogic.Senders
{
    public interface IEmailSender
    {
        Task SendEmail(string recipientEmail, string recipientName, string subject, string htmlBody);
    }
}