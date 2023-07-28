namespace RGO.Repository.Entities;

public record Input(
    int id,
    int userId,
    int formSubmitId,
    int fieldId,
    string value,
    DateTime createDate);