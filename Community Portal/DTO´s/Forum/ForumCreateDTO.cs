namespace Community_Portal.DTO_s.Forum
{
    public record struct ForumCreateDTO(int UserId, string Title, List<PostCreateDTO> Posts);
}