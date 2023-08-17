namespace RGO.Models
{
    public record FieldDto(
        int id,
        int FormId,
        int Type,
        bool Required,
        string Label,
        string Description,
        string ErrorMessage
        );
}