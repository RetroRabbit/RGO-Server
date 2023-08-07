namespace RGO.Domain.Models
{
    public record UserStackDto(
        int Id,
        int UserId,
        StacksDto Backend,
        StacksDto Frontend,
        StacksDto Database,
        string Description,
        int Status,
        DateTime CreateDate);
}
