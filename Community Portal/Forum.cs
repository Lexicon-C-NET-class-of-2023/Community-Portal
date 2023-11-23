namespace Community_Portal
{
    public class Forum
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<Post>? Posts { get; set; }
    }
}