using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeDataDto(
    int Id,
    int EmployeeId,
    int FieldCodeId,
    string Value);