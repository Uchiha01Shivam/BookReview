namespace BookReviewWeb.Models
{
    public class AnsLike
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public int answerId { get; set; }

        public int? Likes { get; set; } = 0;

        public int? DisLikes { get; set; } = 0;

        public bool? IsLiked { get; set; }

        public virtual Community? Community { get; set; }
    }
}
