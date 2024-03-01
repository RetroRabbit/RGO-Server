using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Handler.Charts;

public class AgeTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly AgeType ageType;
    private EmployeeAddressDto? employeeAddressDto;
    private readonly EmployeeType employeeType;
    private readonly EmployeeTypeDto employeeTypeDto;
    private RoleDto roleDto;

    public AgeTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        ageType = new AgeType();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        employeeType = new EmployeeType(employeeTypeDto);
        roleDto = new RoleDto{ Id = 3, Description = "Employee" };
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                                .Returns(Task.FromResult(employeeTypeDto));
        employeeAddressDto =
            new EmployeeAddressDto
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
    }

    private EmployeeDto CreateEmployee(DateTime dob)
    {
        return new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                               null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Juanro", "JM",
                               "MInne", dob, "South Africa", "South African", "0000080000000", " ",
                               new DateTime(), null, Race.Black, Gender.Male, null,
                               "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                               employeeAddressDto, employeeAddressDto, null, null, null);
    }

    [Fact]
    public void GenerateDataDobTestSuccess()
    {
        var testDate = "05/05/2005";
        var employeeDto = CreateEmployee(Convert.ToDateTime(testDate));

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = ageType.GenerateData(employeeDto, realServiceProvider);

        Assert.Equal("Age 18, ", result);
    }
}
