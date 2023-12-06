namespace Community_Portal.DTO_s
{
    public record struct NewsCreateDTO(int UserId, string Title, List<PostCreateDto> Posts);
}