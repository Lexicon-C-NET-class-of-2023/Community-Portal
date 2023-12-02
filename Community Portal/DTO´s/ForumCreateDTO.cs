namespace Community_Portal.DTO_s
{
    public record struct ForumCreateDTO(int UserId, string Title, List<PostCreateDto> Posts);
}