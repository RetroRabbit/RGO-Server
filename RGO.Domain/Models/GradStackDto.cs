using RGO.Domain.Enums;

namespace RGO.Domain.Models
{
    public record GradStackDto(
        int Id,
        int UserId,
        StacksDto Backend,
        StacksDto Frontend,
        StacksDto Database,
        string Description,
        GradProjectStatus Status,
        DateTime CreateDate);
}
