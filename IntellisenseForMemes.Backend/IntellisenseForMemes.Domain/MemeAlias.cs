using System;
using System.Collections.Generic;
using System.Text;
using IntellisenseForMemes.Domain.Common;

namespace IntellisenseForMemes.Domain
{
    public class MemeAlias : BaseEntity
    {
        protected MemeAlias() { }

        public MemeAlias(string alias, Meme meme)
        {
            Alias = alias;
            Meme = meme;
        }

        public string Alias { get; set; }

        public int MemeId { get; set; }

        public virtual Meme Meme { get; set; }
    }
}
