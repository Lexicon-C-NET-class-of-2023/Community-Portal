namespace Community_Portal.DTO_s.User
{
    public record struct UserCreateDTO(string FirstName, string LastName, string Email, string Password);
}