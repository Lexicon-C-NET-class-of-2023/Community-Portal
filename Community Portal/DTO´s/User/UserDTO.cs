namespace Community_Portal.DTO_s.User
{
    public record struct UserDTO(int Id, DateTime Created, string FirstName, string LastName, string Email);
}