using IntellisenseForMemes.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IntellisenseForMemes.DAL
{
    public class IntellisenseDbContext : IdentityDbContext<User>
    {
        public IntellisenseDbContext(DbContextOptions<IntellisenseDbContext> options)
            : base(options)
        {

        }

        public DbSet<Meme> Memes { get; set; }

        public DbSet<MemeAlias> MemeAliases { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attachment>()
                .HasOne(bm => bm.Creator)
                .WithMany(x => x.CreatedAttachments);

            modelBuilder.Entity<Attachment>()
                .HasOne(bm => bm.Modifier)
                .WithMany(x => x.ModifiedAttachments);

            modelBuilder.Entity<Meme>()
                .HasOne(bm => bm.Creator)
                .WithMany(x => x.CreatedMemes);

            modelBuilder.Entity<Meme>()
                .HasOne(bm => bm.Modifier)
                .WithMany(x => x.ModifiedMemes);

            //modelBuilder.Entity<User>()
            //    .HasMany(bm => bm.CreatedMemes)
            //    .WithOne(x => x.Creator);

            //modelBuilder.Entity<User>()
            //    .HasMany(bm => bm.ModifiedMemes)
            //    .WithOne(x => x.Modifier);
        }
    }
}