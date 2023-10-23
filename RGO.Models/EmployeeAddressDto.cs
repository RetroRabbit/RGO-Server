using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeAddressDto(
    int Id,
    string UnitNumber,
    string ComplexName,
    string StreetNumber,
    string SuburbOrDistrict,
    string City,
    string Country,
    string Province,
    string PostalCode);