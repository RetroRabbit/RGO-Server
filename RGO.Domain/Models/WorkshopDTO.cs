namespace RGO.Domain.Models;

public record WorkshopDto(
    int Id, 
    EventsDto EventId, 
    string Presenter
    );
