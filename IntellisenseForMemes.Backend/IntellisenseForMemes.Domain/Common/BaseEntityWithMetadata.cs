using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellisenseForMemes.Domain.Common
{
    public class BaseEntityWithMetadata<T> : BaseEntity<T>
    {
        protected BaseEntityWithMetadata() { }

        public BaseEntityWithMetadata(User creator)
        {
            Creator = creator;
            Modifier = creator;
            LastModifiedDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }

        public string CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public virtual User Creator { get; set; }

        public string ModifierId { get; set; }

        [ForeignKey("ModifierId")]
        public virtual User Modifier { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public DateTime CreationDate { get; set; }

        protected void UpdateMetadata(User modifier)
        {
            Modifier = modifier;
            LastModifiedDate = DateTime.UtcNow;
        }
    }

    public class BaseEntityWithMetadata : BaseEntityWithMetadata<int>
    {
        protected BaseEntityWithMetadata() { }

        public BaseEntityWithMetadata(User creator) : base(creator)
        { 
        }
    }
}
