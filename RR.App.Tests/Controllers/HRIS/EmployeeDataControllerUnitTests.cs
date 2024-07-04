using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeDataControllerUnitTests
{
    private readonly Mock<IEmployeeDataService> _employeeDataServiceMock;
    private readonly EmployeeDataController _controller;
    private readonly List<EmployeeDataDto> _employeeDataDtoList;
    private readonly EmployeeDataDto _employeeDataDto;

    public EmployeeDataControllerUnitTests()
    {
        _employeeDataServiceMock = new Mock<IEmployeeDataService>();
        _controller = new EmployeeDataController(new AuthorizeIdentityMock(2), _employeeDataServiceMock.Object);

        _employeeDataDtoList = new List<EmployeeDataDto>
        {
             EmployeeDataTestData.EmployeeDataOne.ToDto(),
             EmployeeDataTestData.EmployeeDataTwo.ToDto(),
        };
        _employeeDataDto = EmployeeDataTestData.EmployeeDataOne.ToDto();
    }

    [Fact]
    public async Task GetEmployeeDataReturnsOkResult()
    {
         _employeeDataServiceMock.Setup(x => x.GetAllEmployeeData(_employeeDataDto.Id))
                               .ReturnsAsync(_employeeDataDtoList);

        var result = await _controller.GetEmployeeData(_employeeDataDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<EmployeeDataDto>>(okResult.Value);
        Assert.NotNull(result);
        Assert.Equivalent(_employeeDataDtoList, returnValue);
    }

    [Fact]
    public async Task SaveEmployeeDataReturnsOkResult()
    {
        _employeeDataServiceMock.Setup(x => x.SaveEmployeeData(_employeeDataDto))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.SaveEmployeeData(_employeeDataDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
        Assert.Equal(_employeeDataDto, returnValue);
    }

    [Fact]
    public async Task UpdateEmployeeDataReturnsOkResult()
    {
        _employeeDataServiceMock.Setup(x => x.UpdateEmployeeData(_employeeDataDto))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.UpdateEmployeeData(_employeeDataDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
        Assert.Equal(_employeeDataDto, returnValue);
    }

    [Fact]
    public async Task DeleteEmployeeDataReturnsOkResult()
    {
        _employeeDataServiceMock.Setup(service => service.DeleteEmployeeData(_employeeDataDto.Id))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.DeleteEmployeeData(_employeeDataDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
        Assert.Equal(_employeeDataDto, returnValue);
    }
}