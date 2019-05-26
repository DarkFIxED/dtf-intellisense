using IntellisenseForMemes.Domain;

namespace IntellisenseForMemes.BusinessLogic.Services.Meme.Models
{
    public class AttachmentModel
    {
        public int Id { get; set; }

        public AttachmentTypes Type { get; set; }

        public string Url { get; set; }
    }
}
