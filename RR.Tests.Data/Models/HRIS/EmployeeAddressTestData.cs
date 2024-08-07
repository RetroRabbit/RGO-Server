using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeAddressTestData
{
    public static EmployeeAddress EmployeeAddressOne = new()
    {
        Id = 1,
        EmployeeId = 1,
        UnitNumber = "2",
        ComplexName = "Complex",
        StreetNumber = "2",
        SuburbOrDistrict = "Suburb/District",
        City = "City",
        Country = "Country",
        Province = "Province",
        PostalCode = "1620"
    };

    public static EmployeeAddress EmployeeAddressTwo = new()
    {
        Id = 1,
        EmployeeId = 2,
        UnitNumber = "1",
        ComplexName = "Complex Name 1",
        StreetNumber = "Street Number 1",
        SuburbOrDistrict = "Suburb or District 1",
        City = "City 1",
        Country = "Country 1",
        Province = "Province 1",
        PostalCode = "0001"
    };

    public static EmployeeAddress EmployeeAddressThree = new()
    {
        Id = 2,
        EmployeeId = 3,
        UnitNumber = "2",
        ComplexName = "Complex Name 2",
        StreetNumber = "Street Number 2",
        SuburbOrDistrict = "Suburb or District 2",
        City = "City 2",
        Country = "Country 2",
        Province = "Province 2",
        PostalCode = "0002"
    };
    public static EmployeeAddress EmployeeAddressNew = new()
    {
        Id = 0,
        EmployeeId = 4,
        UnitNumber = "56",
        ComplexName = "Complex72",
        StreetNumber = "8",
        SuburbOrDistrict = "Suburb/District",
        City = "City",
        Country = "Country",
        Province = "Province",
        PostalCode = "1620"
    };

    public static EmployeeAddress GetModifiedEmployeeAdressDtoWithAddressId(int employeeId)
    {
        return new EmployeeAddress
        {
            Id = employeeId,
            EmployeeId = 5,
            UnitNumber = "56",
            ComplexName = "Complex72",
            StreetNumber = "8",
            SuburbOrDistrict = "Suburb/District",
            City = "City",
            Country = "Country",
            Province = "Province",
            PostalCode = "1620"
        };
    }
}
