using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeAddressDto(
    int Id,
    EmployeeDto? Employee,
    AddressType AddressType,
    string UnitNumber,
    string ComplexName,
    string StreetNumber,
    string StreetName,
    string Suburb,
    string City,
    string PostalCode);