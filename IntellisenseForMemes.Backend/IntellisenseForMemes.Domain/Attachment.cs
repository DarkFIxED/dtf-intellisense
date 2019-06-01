using System.ComponentModel.DataAnnotations.Schema;
using IntellisenseForMemes.Domain.Common;

namespace IntellisenseForMemes.Domain
{
    public class Attachment : BaseEntityWithMetadata
    {
        protected Attachment() { }

        public Attachment(User creator, string url, AttachmentTypes type, Meme meme) : base(creator)
        {
            AttachmentUrl = url;
            AttachmentType = type;
            Meme = meme;
        }

        public AttachmentTypes AttachmentType { get; protected set; }

        public string AttachmentUrl { get; protected set; }

        public int MemeId { get; set; }

        public Meme Meme { get; set; }

        public string ObjectFromDtfInJson { get; protected set; }

        public void UpdateObjectFromDtfInJson(string objectFromDtfInJson)
        {
            ObjectFromDtfInJson = objectFromDtfInJson;
        }
    }
}
