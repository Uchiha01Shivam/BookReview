using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookReviewWeb.Models;

namespace BookReviewWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<Review> Reviews { get; set; } 
        
        public DbSet<FAQ> FAQs { get; set; }// Add DbSet for reviews
        public DbSet<Community> Community { get; set; }

        public DbSet<Discussion> discussions { get; set; }
        public DbSet<Likes> likes { get; set; } 

        public DbSet<AnsLike> AnsLike { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships between Book and Review entities if needed.
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId);

            modelBuilder.Entity<Community>()
               .HasOne(r => r.Discussion)
               .WithMany(b => b.Community)
               .HasForeignKey(r => r.QuestionId);

            modelBuilder.Entity<Likes>()
            .HasOne(r => r.Discussion)
            .WithMany(b => b.Likes)
            .HasForeignKey(r => r.DiscussionId);

            modelBuilder.Entity<AnsLike>()
        .HasOne(r => r.Community)
        .WithMany(b => b.AnsLikes)
        .HasForeignKey(r => r.answerId);
        }
    }
}
