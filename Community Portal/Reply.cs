namespace Community_Portal
{
    public class Reply
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}