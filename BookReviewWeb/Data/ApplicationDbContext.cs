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
        public DbSet<Review> Reviews { get; set; } // Add DbSet for reviews

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships between Book and Review entities if needed.
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId);
        }
    }
}
