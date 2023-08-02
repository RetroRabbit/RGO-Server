namespace RGO.Domain.Models
{
    public record EventsDto(
        int id, 
        int groupId, 
        string title, 
        string description, 
        int userType, 
        DateTime startDate, 
        DateTime endDate, 
        int eventType
        );
}
