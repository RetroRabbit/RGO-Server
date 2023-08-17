namespace RGO.Models
{
    public record GradEventsDto(
        int Id,
        int? GroupId,
        string Title,
        string Description,
        int UserType,
        DateTime StartDate,
        DateTime EndDate,
        int EventType
        );
}