using System.Collections.Generic;
using System.Threading.Tasks;
using IntellisenseForMemes.BusinessLogic.Services.Meme.Models;

namespace IntellisenseForMemes.BusinessLogic.Services.Meme
{
    public interface IMemeService
    {
        Task<List<MemeModel>> GetMemes();
        Task<MemeModel> GetMeme(int memeId);
        Task<int> CreateMeme(Domain.User creator, MemeModel meme);
        Task UpdateMeme(Domain.User modifier, MemeModel meme);
        Task<List<MemeBriefModel>> SearchMemes(string option);
        Task<string> GetDtfAttachmentByMemeName(string memeName);
        Task NormalizedMemes();
    }
}