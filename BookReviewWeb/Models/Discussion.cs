namespace BookReviewWeb.Models
{
    public class Discussion
    {
        public int Id { get; set; } 
        public string User { get; set; }   
        public string question { get; set; }
     
        public virtual ICollection<Community>? Community { get; set; }

        public virtual ICollection<Likes>? Likes { get; set; }

    }
}
