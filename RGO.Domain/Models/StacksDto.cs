namespace RGO.Domain.Models;

public record StacksDto
(
    int Id,
    string Title,
    string Description,
    string Url,
    int StackType
);