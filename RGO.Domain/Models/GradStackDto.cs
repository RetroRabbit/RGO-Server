namespace RGO.Domain.Models
{
    public record GradStackDto(
        int Id,
        int UserId,
        StacksDto Backend,
        StacksDto Frontend,
        StacksDto Database,
        string Description,
        GradStackStatus Status,
        DateTime CreateDate);
}
