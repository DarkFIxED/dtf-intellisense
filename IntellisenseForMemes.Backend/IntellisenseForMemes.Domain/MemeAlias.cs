using IntellisenseForMemes.Common.Extensions;
using IntellisenseForMemes.Domain.Common;

namespace IntellisenseForMemes.Domain
{
    public class MemeAlias : BaseEntity
    {
        protected MemeAlias() { }

        public MemeAlias(string alias, Meme meme)
        {
            Alias = alias;
            NormalizedAlias = alias.NormalizeWords();
            Meme = meme;
        }

        public string Alias { get; set; }

        public string NormalizedAlias { get; set; }

        public int MemeId { get; set; }

        public virtual Meme Meme { get; set; }
    }
}
