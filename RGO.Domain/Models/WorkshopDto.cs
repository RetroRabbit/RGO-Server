namespace RGO.Domain.Models
{
    public record WorkshopDto
        (
            int id, 
            EventsDto eventId, 
            string presenter
        );
}
