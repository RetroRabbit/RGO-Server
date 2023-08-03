namespace RGO.Domain.Models;

public record EventsDto(
    int Id, 
    int? GroupId, 
    string Title, 
    string Description, 
    int UserType, 
    DateTime StartDate, 
    DateTime EndDate, 
    int EventType
    );
