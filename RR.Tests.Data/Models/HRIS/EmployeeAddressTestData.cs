using HRIS.Models;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeAddressTestData
{
    public static EmployeeAddressDto EmployeeAddressDto = new EmployeeAddressDto
    {
        Id = 1,
        UnitNumber = "2",
        ComplexName = "Complex",
        StreetNumber = "2",
        SuburbOrDistrict = "Suburb/District",
        City = "City",
        Country = "Country",
        Province = "Province",
        PostalCode = "1620"
    };

    public static EmployeeAddressDto EmployeeAddressDto2 = new EmployeeAddressDto
    {
        Id = 1,
        UnitNumber = "1",
        ComplexName = "Complex Name 1",
        StreetNumber = "Street Number 1",
        SuburbOrDistrict = "Suburb or District 1",
        City = "City 1",
        Country = "Country 1",
        Province = "Province 1",
        PostalCode = "0001"
    };

    public static EmployeeAddressDto EmployeeAddressDto3 = new EmployeeAddressDto
    {
        Id = 2,
        UnitNumber = "2",
        ComplexName = "Complex Name 2",
        StreetNumber = "Street Number 2",
        SuburbOrDistrict = "Suburb or District 2",
        City = "City 2",
        Country = "Country 2",
        Province = "Province 2",
        PostalCode = "0002"
    };
}
