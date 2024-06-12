using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class AuthServiceUnitTest
{
    private readonly AuthService _authService;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IConfiguration> _configuration;
    private readonly Mock<IEmployeeRoleService> _employeeRoleServiceMock;
    private readonly Mock<IEmployeeService> _employeeService;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IRoleAccessLinkService> _roleAccessLinkService;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private EmployeeAddressDto? employeeAddressDto;
    private readonly EmployeeType? employeeType1;
    private readonly EmployeeType? employeeType2;
    private readonly EmployeeTypeDto employeeTypeDto1;
    private readonly EmployeeTypeDto? employeeTypeDto2;
    private readonly ErrorLoggingService _errorLoggingService;

    private EmployeeTypeService? employeeTypeService;
    
    public AuthServiceUnitTest()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _roleAccessLinkService = new Mock<IRoleAccessLinkService>();
        _employeeService = new Mock<IEmployeeService>();
        _configuration = new Mock<IConfiguration>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        _authServiceMock = new Mock<IAuthService>();
        _errorLoggingService = new ErrorLoggingService(_unitOfWork.Object);

        employeeTypeService = new EmployeeTypeService(_unitOfWork.Object);
        employeeTypeDto1 = new EmployeeTypeDto{ Id = 3, Name = "Developer" };
        employeeTypeDto2 = new EmployeeTypeDto{ Id = 7, Name = "People Champion" };
        employeeType1 = new EmployeeType(employeeTypeDto1);
        employeeType2 = new EmployeeType(employeeTypeDto2);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType1.Name!))
                                .Returns(Task.FromResult(employeeTypeDto1));
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType2.Name!))
                                .Returns(Task.FromResult(employeeTypeDto2));

        employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };
        _authService = new AuthService(_errorLoggingService);
    }


}
