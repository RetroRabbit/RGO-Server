using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeDataDto(
    int Id,
    EmployeeDto Employee,
    EmployeeDataType DataType,
    EmployeeDataSubType SubType,
    string Value);