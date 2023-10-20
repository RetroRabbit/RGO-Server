using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeAddressDto(
    int Id,
    int EmployeeId,
    string UnitNumber,
    string ComplexName,
    string StreetNumber,
    string SuburbOrDistrict,
    string Country,
    string Province,
    string PostalCode);