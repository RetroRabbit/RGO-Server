﻿using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeAddressServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly EmployeeAddressService _employeeAddressService;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeAddressServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeAddressService = new EmployeeAddressService(_dbMock.Object, _errorLoggingServiceMock.Object);
        _errorLoggingService = new ErrorLoggingService(_dbMock.Object);
    }

    private EmployeeAddressDto CreateAddress(int id = 0)
    {
        return new EmployeeAddressDto
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
        var address = CreateAddress(1);

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync((EmployeeAddressDto?)null);

        var result = await _employeeAddressService.CheckIfExists(address);

        Assert.False(result);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        var address = CreateAddress(1);

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address);

        var result = await _employeeAddressService.CheckIfExists(address);

        Assert.True(result);
    }

    [Fact]
    public async Task CheckIfExistPassFail()
    {
        var address = CreateAddress(0);

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address);

        var result = await _employeeAddressService.CheckIfExists(address);

        Assert.False(result);
    }

    [Fact]
    public async Task GetFailTest()
    {
        var address = CreateAddress(1);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());
        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.Get(address));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var address = CreateAddress(1);

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeAddress.FirstOrDefault(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(address);

        var result = await _employeeAddressService.Get(address);

        Assert.NotNull(result);
        Assert.Equal(address, result);
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var address = CreateAddress(1);

        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address);

        _dbMock.Setup(x => x.EmployeeAddress.FirstOrDefault(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(address);

        _dbMock.Setup(x => x.EmployeeAddress.Get(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .Returns(new List<EmployeeAddress> { new(address) }.AsQueryable().BuildMock());

        _dbMock.Setup(x => x.EmployeeAddress.Delete(It.IsAny<int>())).Returns(Task.FromResult(address));

        var result = await _employeeAddressService.Delete(address.Id);

        Assert.Equal(address, result);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        var address = CreateAddress(1);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());
        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address);
        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.Save(address));
    }

    [Fact]
    public async Task SavePassTest()
    {
        var address = CreateAddress(1);

        _dbMock.Setup(x => x.EmployeeAddress.Add(It.IsAny<EmployeeAddress>())).Returns(Task.FromResult(address));

        var result = await _employeeAddressService.Save(address);

        Assert.Equal(address, result);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        var address = CreateAddress(1);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());
        _dbMock.Setup(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync((EmployeeAddressDto?)null);

        await Assert.ThrowsAsync<Exception>(() => _employeeAddressService.Update(address));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        var address = CreateAddress(1);

        _dbMock.SetupSequence(x => x.EmployeeAddress.GetById(It.IsAny<int>())).ReturnsAsync(address)
               .ReturnsAsync(address);
        _dbMock.Setup(x => x.EmployeeAddress.Any(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(true);


        _dbMock.Setup(x => x.EmployeeAddress.Update(It.IsAny<EmployeeAddress>())).Returns(Task.FromResult(address));

        var result = await _employeeAddressService.Update(address);

        Assert.Equal(address, result);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var address = CreateAddress(1);

        _dbMock.Setup(x => x.EmployeeAddress.GetAll(It.IsAny<Expression<Func<EmployeeAddress, bool>>>()))
               .ReturnsAsync(new List<EmployeeAddressDto> { address });

        var result = await _employeeAddressService.GetAll();

        Assert.NotNull(result);
        Assert.Equal(address, result.First());
    }
}