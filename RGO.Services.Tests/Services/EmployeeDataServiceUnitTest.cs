using HRIS.Models;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class EmployeeDataServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeDataDto _employeeDataDto;
    private readonly EmployeeDataDto _employeeDataDto2;
    private readonly EmployeeDataService _employeeDataService;

    public EmployeeDataServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeDataService = new EmployeeDataService(_dbMock.Object);
        _employeeDataDto = new EmployeeDataDto(0,
                                               0,
                                               0,
                                               "string"
                                              );
        _employeeDataDto2 = new EmployeeDataDto(0,
                                                1,
                                                1,
                                                "string"
                                               );
    }

    [Fact]
    public async Task GetAllEmployeeDataTest()
    {
        var employee = new List<EmployeeDataDto> { _employeeDataDto };

        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).Returns(Task.FromResult(employee));
        var result = await _employeeDataService.GetAllEmployeeData(_employeeDataDto.Id);

        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.Equal(employee, result);
    }

    [Fact]
    public async Task GetEmployeeDataTest()
    {
        var employee = new List<EmployeeDataDto> { _employeeDataDto };

        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).Returns(Task.FromResult(employee));
        var result = await _employeeDataService.GetEmployeeData(_employeeDataDto.Id, _employeeDataDto.Value);

        Assert.NotNull(result);
        Assert.Equal(_employeeDataDto, result);
        _dbMock.Verify(x => x.EmployeeData.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeDataTest()
    {
        var employee = new List<EmployeeDataDto> { _employeeDataDto };
        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).Returns(Task.FromResult(employee));

        _dbMock.Setup(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()))
               .Returns(Task.FromResult(_employeeDataDto));

        var result = await _employeeDataService.SaveEmployeeData(_employeeDataDto2);
        Assert.NotNull(result);
        Assert.Equal(_employeeDataDto, result);
        _dbMock.Verify(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeDataTest()
    {
        var employee = new List<EmployeeDataDto> { _employeeDataDto };
        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).Returns(Task.FromResult(employee));

        _dbMock.Setup(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()))
               .Returns(Task.FromResult(_employeeDataDto));

        var result = await _employeeDataService.UpdateEmployeeData(_employeeDataDto);
        Assert.NotNull(result);
        Assert.Equal(_employeeDataDto, result);
        _dbMock.Verify(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeDataTest()
    {
        var employee = new List<EmployeeDataDto> { _employeeDataDto };
        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).Returns(Task.FromResult(employee));

        _dbMock.Setup(x => x.EmployeeData.Delete(It.IsAny<int>()))
               .Returns(Task.FromResult(_employeeDataDto));

        var result = await _employeeDataService.DeleteEmployeeData(_employeeDataDto.Id);
        Assert.NotNull(result);
        Assert.Equal(_employeeDataDto, result);
        _dbMock.Verify(r => r.EmployeeData.Delete(It.IsAny<int>()), Times.Once);
    }
}