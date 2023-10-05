namespace BookReviewWeb.Models
{
    public class Community
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public int QuestionId { get; set; }

        public string Answer {  get; set; }

      

        public virtual Discussion? Discussion { get; set; }
        public virtual ICollection<AnsLike>? AnsLikes { get; set; }

    }
}
