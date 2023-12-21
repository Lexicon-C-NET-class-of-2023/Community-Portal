namespace Community_Portal.DTO_s.News
{
    public record struct NewsCreateDTO(int UserId, string Title, List<PostCreateDTO> Posts);
}