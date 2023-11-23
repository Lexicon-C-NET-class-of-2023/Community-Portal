namespace Community_Portal
{
    public class User
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}