using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IntellisenseForMemes.Domain
{
    public class User : IdentityUser
    {
        public string Login { get; set; }

        public virtual ICollection<Attachment> CreatedAttachments { get; set; }

        public virtual ICollection<Attachment> ModifiedAttachments { get; set; }

        public virtual ICollection<Meme> CreatedMemes { get; set; }

        public virtual ICollection<Meme> ModifiedMemes { get; set; }
    }
}
