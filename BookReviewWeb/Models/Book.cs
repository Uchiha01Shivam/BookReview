using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReviewWeb.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UploaderUsername { get; set; }
        public string Author { get; set; }

        public string Publication { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; } // Collection of reviews for the book

        // Other properties...
        public int? AuthorCredibility {  get; set; }
        public int? PublisherCreibility { get; set; }
        public int? InGeneral {  get; set; }
        public int? PhysicalAppearance { get; set; }
        public int? SubjectMatter { get; set; }
        public int? Language { get; set; }
        public int? Illustration { get; set; }
        public int? OverAll { get; set; }

        public string? Imagepath { get; set; }
        [NotMapped] // Specify that this property is not mapped to the database
      
        public IFormFile? ImageFile { get; set; }

    }
}
