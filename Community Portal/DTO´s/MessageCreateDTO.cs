namespace Community_Portal.DTO_s
{
    public record struct MessageCreateDTO(DateTime Created, int Recipient, string Content);  
}