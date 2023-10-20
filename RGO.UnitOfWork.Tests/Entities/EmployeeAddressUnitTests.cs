using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeAddressUnitTests
{
    private EmployeeDto _employee;

    public EmployeeAddressUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");

        _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null);
    }

    public EmployeeAddress CreateEmployeeAddress(EmployeeDto? employee = null)
    {
        EmployeeAddress employeeAddress = new EmployeeAddress
        {
            Id = 1,
            EmployeeId = employee != null ? employee.Id : 1,
            UnitNumber = "1",
            ComplexName = "Complex",
            StreetNumber = "1",
            SuburbOrDistrict = "Suburb/District",
            Country = "Country",
            Province = "Province",
            PostalCode = "1620"
        };

        if (employee != null)
            employeeAddress.Employee = new Employee(employee, employee.EmployeeType);

        return employeeAddress;
    }

    [Fact]
    public void EmployeeAddressTest()
    {
        var employeeAddress = new EmployeeAddress();
        Assert.IsType<EmployeeAddress>(employeeAddress);
        Assert.NotNull(employeeAddress);
    }

    [Fact]
    public void EmployeeAddressToDTO()
    {
        var employeeAddress = CreateEmployeeAddress(_employee);
        var employeeAddressDto = employeeAddress.ToDto();

        Assert.Equal(employeeAddress.Id, employeeAddressDto.Id);
        Assert.Equal(employeeAddress.EmployeeId, employeeAddressDto.EmployeeId);
        Assert.Equal(employeeAddress.UnitNumber, employeeAddressDto.UnitNumber);
        Assert.Equal(employeeAddress.ComplexName, employeeAddressDto.ComplexName);
        Assert.Equal(employeeAddress.StreetNumber, employeeAddressDto.StreetNumber);
        Assert.Equal(employeeAddress.SuburbOrDistrict, employeeAddressDto.SuburbOrDistrict);
        Assert.Equal(employeeAddress.Country, employeeAddressDto.Country);
        Assert.Equal(employeeAddress.Province, employeeAddressDto.Province);
        Assert.Equal(employeeAddress.PostalCode, employeeAddressDto.PostalCode);
    }
}
