using System.Threading.Tasks;

namespace IntellisenseForMemes.BusinessLogic.Senders.DtfSender
{
    public interface IDtfSender : IBaseSender
    {
        Task<string> GetArticles(int count, int offset);

        Task<string> UploadAttachment(string attachmentUrl);
        Task<string> PostComment(int articleId, int replyTo, string text, string dtfAttachmentObject);
    }
}