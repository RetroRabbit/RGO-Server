namespace RGO.Repository.Entities;

public record Input(
    int id,
    users userId,
    formSubmits formSubmitId,
    fields fieldId,
    string value,
    DateTime createDate);