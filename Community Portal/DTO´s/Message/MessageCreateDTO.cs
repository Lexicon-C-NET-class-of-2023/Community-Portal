namespace Community_Portal.DTO_s.Message
{
    public record struct MessageCreateDTO(int UserId, int Recipient, string Content);  
}