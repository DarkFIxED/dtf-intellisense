using System.Collections.Generic;

namespace IntellisenseForMemes.BusinessLogic.Services.Meme.Models
{
    public class MemeModel
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public List<AliasModel> Aliases { get; set; }

        public List<AttachmentModel> Attachments { get; set; }
    }
}
