namespace RGO.Models
{
    public record SocialDto(
    int Id,
    int UserId,
    string Discord,
    string CodeWars,
    string GitHub,
    string LinkedIn
    );
}