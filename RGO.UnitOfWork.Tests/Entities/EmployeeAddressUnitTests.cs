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
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");
    }

    public EmployeeAddress CreateEmployeeAddress(EmployeeDto? employee = null)
    {
        EmployeeAddress employeeAddress = new EmployeeAddress
        {
            Id = 1,
            EmployeeId = 1,
            AddressType = AddressType.Complex,
            UnitNumber = "1",
            ComplexName = "Complex",
            StreetNumber = "1",
            StreetName = "Street",
            Suburb = "Suburb",
            City = "City",
            PostalCode = "PostalCode"
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
        var employeeAdress = CreateEmployeeAddress(employee: _employee);
        var dto = employeeAdress.ToDto();

        Assert.NotNull(dto.Employee);
        Assert.Equal(employeeAdress.EmployeeId, dto.Employee!.Id);


        employeeAdress = CreateEmployeeAddress();
        dto = employeeAdress.ToDto();

        Assert.Null(dto.Employee);

        employeeAdress = CreateEmployeeAddress(employee: _employee);
        dto = employeeAdress.ToDto();

        var conversion = new EmployeeAddress(dto);

        Assert.Null(conversion.Employee);
    }
}
