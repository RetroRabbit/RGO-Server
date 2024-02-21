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

namespace RGO.Tests.Services;

public class ChartServiceUnitTests
{
    private readonly Mock<IEmployeeService> _employeeService;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IServiceProvider> _services;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private EmployeeAddressDto? employeeAddressDto;
    private readonly EmployeeType employeeType1;
    private readonly EmployeeType employeeType2;
    private readonly EmployeeTypeDto employeeTypeDto1;
    private readonly EmployeeTypeDto employeeTypeDto2;

    public ChartServiceUnitTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _employeeService = new Mock<IEmployeeService>();
        _services = new Mock<IServiceProvider>();

        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        employeeTypeDto1 = new EmployeeTypeDto(3, "Developer");
        employeeTypeDto2 = new EmployeeTypeDto(7, "People Champion");
        employeeType1 = new EmployeeType(employeeTypeDto1);
        employeeType2 = new EmployeeType(employeeTypeDto2);
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType1.Name))
                                .Returns(Task.FromResult(employeeTypeDto1));
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType2.Name))
                                .Returns(Task.FromResult(employeeTypeDto2));
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
    }

    [Fact]
    public async Task GetAllChartsTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        _unitOfWork.Setup(u => u.Chart.GetAll(null)).ReturnsAsync(new List<ChartDto>());

        var result = await chartService.GetAllCharts();

        Assert.NotNull(result);
        Assert.IsType<List<ChartDto>>(result);
        _unitOfWork.Verify(u => u.Chart.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task CreateChartTest()
    {
        var roles = new List<string> { "Developer, Designer" };
        var dataTypes = new List<string> { "Gender, Race, Age" };
        var chartName = "TestChart";
        var chartType = "Pie";

        EmployeeTypeDto developerEmployeeTypeDto = new(1, "Developer");
        EmployeeTypeDto designerEmployeeTypeDto = new(2, "Designer");

        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                      null, false, "None", 4, developerEmployeeTypeDto, "Notes", 1, 28, 128, 100000,
                                      "Dotty", "D",
                                      "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
                                      new DateTime(), null, Race.Black, Gender.Female, null,
                                      "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null,
                                      employeeAddressDto, employeeAddressDto, null, null, null);
        EmployeeDto desEmployeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                         null, false, "None", 4, designerEmployeeTypeDto, "Notes", 1, 28, 128, 100000,
                                         "Dotty", "D",
                                         "Missile", new DateTime(), "South Africa", "South African", "0000000000000",
                                         " ",
                                         new DateTime(), null, Race.Black, Gender.Female, null,
                                         "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null,
                                         employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, developerEmployeeTypeDto),
            new(desEmployeeDto, designerEmployeeTypeDto)
        };

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto,
            desEmployeeDto
        };

        var chartDto = new ChartDto(
                                    1,
                                    chartName,
                                    chartType,
                                    dataTypes,
                                    new List<string> { "Male", "Female" },
                                    new List<int> { 1, 1 }
                                   );

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeDtoList);

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeList.AsQueryable().BuildMock());

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.CreateChart(dataTypes, roles, chartName, chartType);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);
    }

    [Fact]
    public async Task CreateChartTestAll()
    {
        var roles = new List<string> { "All" };
        var dataTypes = new List<string> { "Gender, Race, Age" };
        var chartName = "TestChart";
        var chartType = "Pie";

        EmployeeTypeDto devEmployeeTypeDto = new(1, "Developer");
        EmployeeTypeDto desEmployeeTypeDto = new(2, "Designer");

        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                      null, false, "None", 4, devEmployeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                      "D",
                                      "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
                                      new DateTime(), null, Race.Black, Gender.Female, null,
                                      "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null,
                                      employeeAddressDto, employeeAddressDto, null, null, null);
        EmployeeDto desEmployeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                         null, false, "None", 4, desEmployeeTypeDto, "Notes", 1, 28, 128, 100000,
                                         "Dotty", "D",
                                         "Missile", new DateTime(), "South Africa", "South African", "0000000000000",
                                         " ",
                                         new DateTime(), null, Race.Black, Gender.Female, null,
                                         "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null,
                                         employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, devEmployeeTypeDto),
            new(desEmployeeDto, desEmployeeTypeDto)
        };

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto,
            desEmployeeDto
        };

        var chartDto = new ChartDto(
                                    1,
                                    chartName,
                                    chartType,
                                    dataTypes,
                                    new List<string> { "Male", "Female" },
                                    new List<int> { 1, 1 }
                                   );

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeDtoList);

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeList.AsQueryable().BuildMock());

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.CreateChart(dataTypes, roles, chartName, chartType);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);
    }

    [Fact]
    public async Task GetChartDataTest()
    {
        var dataType = new List<string> { "Gender", "Race" };

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        EmployeeType employeeType = new(employeeTypeDto);
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                      null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                      "D",
                                      "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
                                      new DateTime(), null, Race.Black, Gender.Female, null,
                                      "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null,
                                      employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeList = new List<EmployeeDto>
        {
            employeeDto
        };

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeList);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.GetChartData(dataType);

        Assert.NotNull(result);
        Assert.IsType<ChartDataDto>(result);
    }

    [Fact]
    public async Task DeleteChartTest()
    {
        var chartId = 1;
        var expectedChartDto = new ChartDto(chartId,
                                            "Test",
                                            "Pie",
                                            new List<string> { "Gender", "Race" },
                                            new List<string> { "Male", "Female" },
                                            new List<int> { 1, 1 });

        _unitOfWork.Setup(u => u.Chart.Delete(chartId)).ReturnsAsync(expectedChartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.DeleteChart(chartId);

        Assert.NotNull(result);
        Assert.IsType<ChartDto>(result);
        Assert.Equal(expectedChartDto.Id, result.Id);
        _unitOfWork.Verify(x => x.Chart.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateChartTest()
    {
        var chartDtoToUpdate = new ChartDto(1,
                                            "Update",
                                            "Pie",
                                            new List<string> { "Gender", "Race" },
                                            new List<string> { "Male", "Female" },
                                            new List<int> { 1, 1 }
                                           );

        var existingCharts = new List<ChartDto>
        {
            new(
                1,
                "Existing Chart",
                "Existing Type",
                chartDtoToUpdate.DataTypes,
                chartDtoToUpdate.Labels,
                chartDtoToUpdate.Data
               )
        };

        _unitOfWork.Setup(x => x.Chart.GetAll(null)).Returns(Task.FromResult(existingCharts));

        _unitOfWork.Setup(x => x.Chart.Update(It.IsAny<Chart>()))
                   .Returns(Task.FromResult(chartDtoToUpdate));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.UpdateChart(chartDtoToUpdate);

        Assert.NotNull(result);
        Assert.Equal(chartDtoToUpdate, result);
        _unitOfWork.Verify(x => x.Chart.Update(It.IsAny<Chart>()), Times.Once);
    }

    [Fact]
    public void GetColumnsFromTableTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var columnNames = chartService.GetColumnsFromTable();

        Assert.NotNull(columnNames);
        Assert.NotEmpty(columnNames);
    }

    [Fact]
    public async Task ExportCsvAsyncTest()
    {
        var employeeDto = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                          1, false, "None", 3, employeeTypeDto1, "Notes", 1, 28, 128, 100000, "Estiaan",
                                          "MT",
                                          "Britz", new DateTime(), "South Africa", "South African", "0000080000000",
                                          " ",
                                          new DateTime(), null, Race.Black, Gender.Male, null,
                                          "test1@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                                          employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var dataTypeList = new List<string> { "Gender", "Race", "Age" };
        var propertyNames = new List<string>();

        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(Task.FromResult(employeeDtoList));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);
        var result = await chartService.ExportCsvAsync(dataTypeList);
        var expectedResult = new byte[]
        {
            70, 105, 114, 115, 116, 32, 78, 97, 109, 101, 44, 76, 97, 115, 116, 32, 78, 97, 109, 101,
            44, 65, 103, 101, 44, 71, 101, 110, 100, 101, 114, 44, 82, 97, 99, 101, 13, 10, 69, 115, 116, 105, 97, 97,
            110, 44, 66, 114, 105,
            116, 122, 44, 65, 103, 101, 32, 50, 48, 50, 51, 44, 77, 97, 108, 101, 44, 66, 108, 97, 99, 107, 13, 10
        };

        Assert.NotNull(result);
        Assert.IsType<byte[]>(result);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task ExportCsvAsyncTestFail()
    {
        var dataTypeList = new List<string> { "", "" };

        var employeeDto = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                          1, false, "None", 3, employeeTypeDto1, "Notes", 1, 28, 128, 100000, "Estiaan",
                                          "MT",
                                          "Britz", new DateTime(), "South Africa", "South African", "0000080000000",
                                          " ",
                                          new DateTime(), null, Race.Black, Gender.Male, null,
                                          "test1@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                                          employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(Task.FromResult(employeeDtoList));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);
        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await chartService.ExportCsvAsync(dataTypeList));

        Assert.Equal("Invalid property name: ", exception.Message);
    }
}