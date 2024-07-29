using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeAddressServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Employee _employee;
    private readonly EmployeeAddressService _employeeAddressService;

    public EmployeeAddressServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeAddressService = new EmployeeAddressService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));

        _employee = EmployeeTestData.EmployeeOne;
    }

    private EmployeeAddress CreateAddress(int id = 1)
    {
        return new EmployeeAddress
        {
            Id = id,
            UnitNumber = "1",
            ComplexName = "Complex",
            StreetNumber = "1",
            SuburbOrDistrict = "Suburb/District",
            City = "City",
            Country = "Country",
            Province = "Province",
            PostalCode = "1620"
        };
    }

    [Fact]
    public async Task CheckIfExistsFail()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync((EmployeeAddress?)null);

        var result = await _employeeAddressService.CheckIfExists(address.ToDto().Id);

        Assert.False(result);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

        var result = await _employeeAddressService.CheckIfExists(address.ToDto().Id);

        Assert.True(result);
    }

    [Fact]
    public async Task CheckIfExistPassFail()
    {
        var address = CreateAddress(0);

        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(false);

        var result = await _employeeAddressService.CheckIfExists(address.ToDto().Id);

        Assert.False(result);
    }

    [Fact]
    public async Task GetFailTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeAddressService.GetById(address.ToDto().Id));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeAddress.FirstOrDefault(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(address);

        _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(_employee);

        var result = await _employeeAddressService.GetById(address.ToDto().Id);

        Assert.NotNull(result);
        Assert.Equivalent(address.ToDto(), result);
    }

    [Fact]
    public async Task GetUnauthorisedTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeAddress.FirstOrDefault(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(address);

        _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(_employee);

        var tempEmployeeService = new EmployeeAddressService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2));

        await Assert.ThrowsAsync<CustomException>(() => tempEmployeeService.GetById(address.ToDto().Id));
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeAddress.FirstOrDefault(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(address);

        _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .Returns(address.ToMockIQueryable());

        _dbMock.Setup(x => x.EmployeeAddress.Delete(It.IsAny<int>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(_employee);

        var result = await _employeeAddressService.Delete(address.Id);

        Assert.Equivalent(address.ToDto(), result);
    }

    [Fact]
    public async Task DeleteFailTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync((EmployeeAddress?)null);

        await Assert.ThrowsAsync<CustomException>(() => _employeeAddressService.Delete(address.Id));
    }

    [Fact]
    public async Task DeleteUnautorisedTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>())).ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeAddress.FirstOrDefault(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(address);

        _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .Returns(address.ToMockIQueryable());

        _dbMock.Setup(x => x.EmployeeAddress.Delete(It.IsAny<int>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(_employee);

        var tempEmployeeService = new EmployeeAddressService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2));

        await Assert.ThrowsAsync<CustomException>(() => tempEmployeeService.Delete(address.Id));
    }

    [Fact]
    public async Task SaveFailTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address);
        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _employeeAddressService.Create(address.ToDto()));
    }

    [Fact]
    public async Task SavePassTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.Add(It.IsAny<EmployeeAddress>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(_employee);

        var result = await _employeeAddressService.Create(address.ToDto());

        Assert.Equivalent(address.ToDto(), result);
    }

    [Fact]
    public async Task SaveUnauthorisedTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.Add(It.IsAny<EmployeeAddress>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(_employee);

        var tempEmployeeService = new EmployeeAddressService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2));

        await Assert.ThrowsAsync<CustomException>(() => tempEmployeeService.Create(address.ToDto()));
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync((EmployeeAddress?)null);

        await Assert.ThrowsAsync<CustomException>(() => _employeeAddressService.Update(address.ToDto()));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        var address = CreateAddress();

        _dbMock.SetupSequence(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address)
               .ReturnsAsync(address);
        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeAddress.Update(It.IsAny<EmployeeAddress>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(_employee);

        var result = await _employeeAddressService.Update(address.ToDto());

        Assert.Equivalent(address.ToDto(), result);
    }

    [Fact]
    public async Task UpdateAuthorisedTest()
    {
        var address = CreateAddress();

        _dbMock.SetupSequence(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address)
               .ReturnsAsync(address);
        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeAddress.Update(It.IsAny<EmployeeAddress>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.Employee.FirstOrDefault(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(_employee);

        var tempEmployeeService = new EmployeeAddressService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2));

        await Assert.ThrowsAsync<CustomException>(() => tempEmployeeService.Update(address.ToDto()));
    }

    [Fact]
    public async Task GetAllTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.GetAll(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(new List<EmployeeAddress> { address });

        var result = await _employeeAddressService.GetAll();

        Assert.NotNull(result);
        Assert.Equivalent(address.ToDto(), result.First());
    }

    [Fact]
    public async Task GetAllUnauthorisedTest()
    {
        var address = CreateAddress();

        _dbMock.Setup(x => x.EmployeeAddress.GetAll(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(new List<EmployeeAddress> { address });

        var tempEmployeeService = new EmployeeAddressService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2));

        await Assert.ThrowsAsync<CustomException>(() => tempEmployeeService.GetAll());
    }
}