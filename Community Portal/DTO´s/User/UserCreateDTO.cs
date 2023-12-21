namespace Community_Portal.DTO_s
{
    public record struct UserCreateDTO(string FirstName, string LastName, string Email, string password);
}