using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeDataDto(
    int Id,
    EmployeeDto? Employee,
    FieldCodeDto? FieldCode,
    string Value);