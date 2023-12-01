namespace Community_Portal.DTO_s
{
    public record struct PostCreateDto(DateTime Created, int UserId, string Content);   
}