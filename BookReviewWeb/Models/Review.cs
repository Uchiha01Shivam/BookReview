namespace BookReviewWeb.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public int BookId { get; set; } // Foreign key to associate the review with a book
        public virtual Book? Book { get; set; } // Navigation property to access the book associated with the review
    }
}
