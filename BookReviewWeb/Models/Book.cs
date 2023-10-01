using System.Collections.Generic;

namespace BookReviewWeb.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UploaderUsername { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; } // Collection of reviews for the book

        // Other properties...
    }
}
