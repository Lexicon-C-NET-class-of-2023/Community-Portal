namespace Community_Portal.DTO_s.Forum
{
    public record struct ForumCreateDTO(string Title, List<PostCreateDTO> Posts);
}