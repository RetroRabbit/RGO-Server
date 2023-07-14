namespace RGO.Repository.Entities;

public record Events(
    int id,
    UserGroup groupId,
    string title,
    string description,
    int userType,
    DateTime startDate,
    DateTime endDate,
    int eventType);
