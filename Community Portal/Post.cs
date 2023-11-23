namespace Community_Portal
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public List<Reply>? Replies { get; set; }
        public string Content { get; set; }
    }
}