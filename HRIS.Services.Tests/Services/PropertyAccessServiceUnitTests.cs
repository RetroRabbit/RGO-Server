using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Update;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class PropertyAccessServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly Mock<IEmployeeDataService> _employeeDataService;
    private readonly Mock<IEmployeeRoleService> _employeeRoleService;
    private readonly Mock<IPropertyAccessService> _propertyAccessService;
    private readonly Mock<IEmployeeService> _employeeService;

    public PropertyAccessServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeDataService = new Mock<IEmployeeDataService>();
        _employeeRoleService = new Mock<IEmployeeRoleService>();
        _propertyAccessService = new Mock<IPropertyAccessService>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeService = new Mock<IEmployeeService>();
        propertyAccessService = new PropertyAccessService(_dbMock.Object, _employeeRoleService.Object, _employeeDataService.Object, _employeeService.Object, _errorLoggingServiceMock.Object);

    }

    public readonly PropertyAccessService propertyAccessService;

    [Fact]
    public async Task GetAllTest()
    {
        _dbMock.Setup(p => p.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.AsQueryable().BuildMock());

        var result = await propertyAccessService.GetAll();

        Assert.NotNull(result);
        Assert.IsType<List<PropertyAccessDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Id)));
        _dbMock.Verify(p => p.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()), Times.Once);
    }

    [Fact]
    public void GetAccessListByEmployeeIdTest()
    {
        _dbMock.Setup(e => e.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.AsQueryable().BuildMock());

        var result = propertyAccessService.GetAccessListByEmployeeId(1);

        Assert.NotNull(result);
    }
    [Fact]
    public void GetAccessListByRoleIdTest()
    {
        _dbMock.Setup(e => e.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.AsQueryable().BuildMock());


        var result = propertyAccessService.GetAccessListByRoleId(1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdatePropertyAccess()
    {
        _propertyAccessService.Setup(r => r.UpdatePropertyAccess(PropertyAccessTestData.EmployeeTable1.Id, PropertyAccessLevel.read));
                              
        _dbMock.Setup(r => r.PropertyAccess.Update(It.IsAny<PropertyAccess>()))
               .Returns(Task.FromResult(PropertyAccessTestData.EmployeeTable1.ToDto()));

            await propertyAccessService.UpdatePropertyAccess(PropertyAccessTestData.EmployeeTable1.Id, PropertyAccessLevel.read);
    }
}