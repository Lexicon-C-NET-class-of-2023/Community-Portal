namespace Community_Portal.DTO_s
{
    public record struct MessageCreateDTO(int UserId, int Recipient, string Content);  
}