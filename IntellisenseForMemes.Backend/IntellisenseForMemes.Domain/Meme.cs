using System;
using System.Collections.Generic;
using System.Linq;
using IntellisenseForMemes.Domain.Common;

namespace IntellisenseForMemes.Domain
{
    public class Meme : BaseEntityWithMetadata
    {
        protected Meme()
        {
        }

        public Meme(User creator, string name) : base(creator)
        {
            Name = name;
        }

        public string Name { get; protected set; }

        public virtual ICollection<MemeAlias> Aliases { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

        public void AddAttachments(List<Attachment> attachments)
        {
            if (attachments == null || !attachments.Any())
            {
                throw new ArgumentException("Attachments must be");
            }
            Attachments = attachments;
        }


        public void AddAlias(List<MemeAlias> aliases)
        {
            if (aliases == null || !aliases.Any())
            {
                throw new ArgumentException("Aliases must be");
            }
            Aliases = aliases;
        }

        public void UpdateName(User modifier, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name can't be empty");
            }

            Name = name;
            UpdateMetadata(modifier);
        }

        public void UpdateAttachments(User modifier, List<Attachment> attachments)
        {
            if (attachments == null || !attachments.Any())
            {
                throw new ArgumentException("Attachments must be");
            }

            var attachmentIds = attachments.Select(a => a.Id);
            var existAttachmentIds = Attachments.Select(a => a.Id);
            var toAdd = attachments.Where(a => !existAttachmentIds.Contains(a.Id));
            var toRemove = Attachments.Where(a => !attachmentIds.Contains(a.Id)).ToList();

            foreach (var removingAttachment in toRemove)
            {
                Attachments.Remove(removingAttachment);
            }

            foreach (var addingAttachment in toAdd)
            {
                Attachments.Add(addingAttachment);
            }

            UpdateMetadata(modifier);
        }

        public void UpdateAliases(User modifier, List<MemeAlias> aliases)
        {
            if (aliases == null || !aliases.Any())
            {
                throw new ArgumentException("Aliases must be");
            }

            var aliasesIds = aliases.Select(a => a.Id);
            var existAliasesIds = Aliases.Select(a => a.Id);
            var toAdd = aliases.Where(a => !existAliasesIds.Contains(a.Id));
            var toRemove = Aliases.Where(a => !aliasesIds.Contains(a.Id)).ToList();

            foreach (var removingAlias in toRemove)
            {
                Aliases.Remove(removingAlias);
            }

            foreach (var addingAlias in toAdd)
            {
                Aliases.Add(addingAlias);
            }

            UpdateMetadata(modifier);
        }
    }
}
