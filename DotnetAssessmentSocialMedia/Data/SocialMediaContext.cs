using DotnetAssessmentSocialMedia.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotnetAssessmentSocialMedia.Data
{
    public class SocialMediaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<TweetHashtag> TweetHashtags { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Mention> Mentions { get; set; }
        public DbSet<Follow> Follows { get; set; }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
            : base(options) 
        { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TweetHashtag>()
                .HasKey(th => new { th.TweetId, th.HashtagLabel });
            modelBuilder.Entity<TweetHashtag>()
                .HasOne(th => th.Tweet)
                .WithMany(t => t.TweetHashtags)
                .HasForeignKey(th => th.TweetId);
            modelBuilder.Entity<TweetHashtag>()
                .HasOne(th => th.Hashtag)
                .WithMany(h => h.TweetHashtags)
                .HasForeignKey(th => th.HashtagLabel);

            modelBuilder.Entity<Like>()
                .HasKey(like => new { like.TweetId, like.UserId });

            modelBuilder.Entity<Mention>()
                .HasKey(mention => new { mention.TweetId, mention.UserId });

            modelBuilder.Entity<Follow>()
                .HasKey(follow => new { follow.FollowedUserId, follow.FollowerUserId });
        }
    }
}