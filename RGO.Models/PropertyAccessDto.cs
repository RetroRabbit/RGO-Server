namespace RGO.Models;

public record PropertyAccessDto(
    int Id,
    RoleDto Role,
    int Condition,
    FieldCodeDto? FieldCode
    );
