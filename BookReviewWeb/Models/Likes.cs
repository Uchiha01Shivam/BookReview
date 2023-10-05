namespace BookReviewWeb.Models
{
    public class Likes
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int DiscussionId { get; set; }
        public bool? IsLiked { get; set; } = false;
        public int? Like { get; set; } = 0;

        public int? DisLike { get; set; } = 0;
        public virtual Discussion? Discussion { get; set; }
    }
}
