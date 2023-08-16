namespace RGO.Domain.Models
{
public record WorkshopDto(
    int Id, 
    GradEventsDto EventId, 
    string Presenter,
    bool Viewable
    );
}