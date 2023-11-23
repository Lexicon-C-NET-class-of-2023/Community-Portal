namespace Community_Portal
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public int Recipient { get; set; }
        public string Content { get; set; }
    }
}