using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender;
using IntellisenseForMemes.BusinessLogic.Services.Meme.Models;
using IntellisenseForMemes.Common.Extensions;
using IntellisenseForMemes.DAL;
using IntellisenseForMemes.Domain;
using Microsoft.EntityFrameworkCore;

namespace IntellisenseForMemes.BusinessLogic.Services.Meme
{
    public class MemeService : IMemeService
    {
        private readonly IRepository<Domain.Meme> _memeRepository;

        private readonly IDtfSender _dtfSender;

        public MemeService(IRepository<Domain.Meme> memeRepository, IDtfSender dtfSender)
        {
            _memeRepository = memeRepository;
            _dtfSender = dtfSender;
        }

        public async Task<List<MemeBriefModel>> SearchMemes(string option)
        {
            var memes = await _memeRepository.AsQueryable()
                .Include(m => m.Aliases)
                .Where(m => string.IsNullOrWhiteSpace(option) || m.Name.Contains(option) || m.Aliases.Any(a => a.Alias.Contains(option)))
                .Select(m => new MemeBriefModel{
                    DisplayingName = m.Name,
                    Aliases = m.Aliases.Select(a => a.Alias).ToList()
                })
                .ToListAsync();

            return memes;
        }

        public async Task<string> GetDtfAttachmentByMemeName(string memeName)
        {
            var normalizedString = memeName.NormalizeWords();

            var attachmentObject = await _memeRepository.AsQueryable()
                .Include(m => m.Attachments)
                .Include(m => m.Aliases)
                .Where(m => normalizedString.Contains(m.NormalizedName) || m.Aliases.Any(a => normalizedString.Contains(a.NormalizedAlias)))
                .Select(m => m.Attachments.FirstOrDefault().ObjectFromDtfInJson)
                .FirstOrDefaultAsync();

            return attachmentObject;
        }

        public async Task<List<MemeModel>> GetMemes()
        {
            var memes = await _memeRepository.AsQueryable()
                .Include(m => m.Attachments)
                .Include(m => m.Aliases)
                .Select(m => new MemeModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Attachments = m.Attachments.Select(a => new AttachmentModel
                    {
                        Type = a.AttachmentType,
                        Id = a.Id,
                        Url = a.AttachmentUrl
                    }).ToList(),
                    Aliases = m.Aliases.Select(al => new AliasModel
                    {
                        Alias = al.Alias,
                        Id = al.Id
                    }).ToList()
                })
                .ToListAsync();

            return memes;
        }

        public async Task<MemeModel> GetMeme(int memeId)
        {
            var memes = await _memeRepository.AsQueryable()
                .Include(m => m.Attachments)
                .Include(m => m.Aliases)
                .Select(m => new MemeModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Attachments = m.Attachments.Select(a => new AttachmentModel
                    {
                        Type = a.AttachmentType,
                        Id = a.Id,
                        Url = a.AttachmentUrl
                    }).ToList(),
                    Aliases = m.Aliases.Select(al => new AliasModel
                    {
                        Alias = al.Alias,
                        Id = al.Id
                    }).ToList()
                })
                .FirstOrDefaultAsync(m => m.Id == memeId);

            return memes;
        }

        public async Task<int> CreateMeme(Domain.User creator, MemeModel meme)
        {
            var newMeme = new Domain.Meme(creator, meme.Name);
            newMeme.AddAttachments(meme.Attachments.Select(a => new Attachment(creator, a.Url, a.Type, newMeme)).ToList());
            foreach (var dbAttachment in newMeme.Attachments)
            {
                var dtfObject = await _dtfSender.UploadAttachment(dbAttachment.AttachmentUrl);
                dbAttachment.UpdateObjectFromDtfInJson(dtfObject);

                if (newMeme.Attachments.Count <= 1) continue;

                // Delay because we can send only 3 request per second
                await Task.Delay(350);
            }

            newMeme.AddAlias(meme.Aliases.Select(a => new MemeAlias(a.Alias, newMeme)).ToList());

            _memeRepository.Create(newMeme);
            await _memeRepository.SaveAsync();

            return newMeme.Id;
        }

        public async Task UpdateMeme(Domain.User modifier, MemeModel meme)
        {
            var memeDb = await _memeRepository.AsQueryable()
                .Include(m => m.Attachments)
                .Include(m => m.Aliases)
                .FirstOrDefaultAsync(m => m.Id == meme.Id);
            memeDb.UpdateName(modifier, meme.Name);

            var newAttachments = meme.Attachments.Select(a => new Attachment(modifier, a.Url, a.Type, memeDb) { Id = a.Id }).ToList();
            memeDb.UpdateAttachments(modifier, newAttachments);

            foreach (var dbAttachment in memeDb.Attachments)
            {
                if (!string.IsNullOrWhiteSpace(dbAttachment.ObjectFromDtfInJson))
                {
                    continue;
                }

                var dtfObject = await _dtfSender.UploadAttachment(dbAttachment.AttachmentUrl);
                dbAttachment.UpdateObjectFromDtfInJson(dtfObject);

                if (memeDb.Attachments.Count <= 1) continue;

                // Delay because we can send only 3 request per second
                await Task.Delay(350);
            }

            var newAliases = meme.Aliases.Select(a => new MemeAlias(a.Alias, memeDb) { Id = a.Id }).ToList();
            memeDb.UpdateAliases(modifier, newAliases);

            _memeRepository.Update(memeDb);
            await _memeRepository.SaveAsync();
        }

        public async Task NormalizedMemes()
        {
            var memes = await _memeRepository.AsQueryable().Include(m => m.Aliases).ToListAsync();
            foreach (var meme in memes)
            {
                meme.Normalize();
            }

            await _memeRepository.SaveAsync();
        }
    }
}
