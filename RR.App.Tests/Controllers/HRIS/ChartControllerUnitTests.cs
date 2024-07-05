using System.Text;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class ChartControllerUnitTests
{
    private readonly ChartsController _controller;
    private readonly Mock<IChartService> _chartServiceMock;
    private readonly List<string> _roles;
    private readonly List<string> _dataTypes;
    private readonly string _chartName;
    private readonly string _chartType;
    private readonly ChartDto _chartDto;
    private readonly List<ChartDto> _chartDtoList;
    private readonly ChartDataDto _chartDataDto;


    public ChartControllerUnitTests()
    {
        _chartServiceMock = new Mock<IChartService>();
        _controller = new ChartsController(_chartServiceMock.Object);
        _roles = new List<string> { "Developer" };
        _dataTypes = new List<string> { "Type1", "Type2", "Type3" };
        _chartName = "Sample Chart";
        _chartType = "Bar";
        var labels = new List<string> { "Label1", "Label2", "Label3" };

        var chartDataSetDto = new ChartDataSetDto
        {
            Label = "Lable 1",
            Data = new List<int> { 10, 20, 30 }
        };

        _chartDto = new ChartDto
        {
            Id = 1,
            Name = _chartName,
            Type = _chartType,
            DataTypes = _dataTypes,
            Labels = labels,
            Roles = new List<string> { "All" },
            Datasets = new List<ChartDataSetDto> { chartDataSetDto }
        };

        _chartDtoList = new List<ChartDto> 
        { 
            _chartDto,
            _chartDto
        };

        _chartDataDto = new ChartDataDto();
    }

    [Fact]
    public async Task GetAllCharts_ReturnsOk_WithCharts()
    {
        _chartServiceMock.Setup(service => service.GetAllCharts())
                         .ReturnsAsync(_chartDtoList);

        var result = await _controller.GetAllCharts();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_chartDtoList, okResult.Value);
    }

    [Fact]
    public async Task GetEmployeeCharts_ReturnsOk_WithCharts()
    {
        var employeeId = 1;
        _chartServiceMock.Setup(service => service.GetEmployeeCharts(employeeId)).ReturnsAsync(_chartDtoList);
        var result = await _controller.GetEmployeeCharts(employeeId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCharts = Assert.IsType<List<ChartDto>>(okResult.Value);
        Assert.Equal(_chartDtoList.Count, returnedCharts.Count);
    }

    [Fact]
    public async Task CreateChart_ReturnsOk_OnSuccess()
    {
        var result = await _controller.CreateChart(_dataTypes, _roles, _chartName, _chartType, 0);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetChartData_ReturnsOk_WithData()
    {
        _chartServiceMock.Setup(service => service.GetChartData(It.IsAny<List<string>>()))
            .ReturnsAsync(_chartDataDto);

        var result = await _controller.GetChartData(_dataTypes);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_chartDataDto, okResult.Value);
    }

    [Fact]
    public async Task UpdateChartData_ReturnsOk_WithUpdatedData()
    {
        _chartServiceMock.Setup(service => service.UpdateChart(It.IsAny<ChartDto>()))
            .ReturnsAsync(_chartDto);

        var result = await _controller.UpdateChartData(_chartDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_chartDto, okResult.Value);
    }

    [Fact]
    public async Task DeleteChart_ReturnsOk_WithDeletedChart()
    {

        _chartServiceMock.Setup(service => service.DeleteChart(1))
            .ReturnsAsync(_chartDto);

        var result = await _controller.DeleteChart(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_chartDto, okResult.Value);
    }

    [Fact]
    public void GetColumns_ReturnsOk_WithColumns()
    {
        var mockColumns = new List<string> { "Disabilitie", "Level", "Age" };

        _chartServiceMock.Setup(service => service.GetColumnsFromTable())
            .Returns(mockColumns.ToArray());

        var result = _controller.GetColumns();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockColumns.ToArray(), okResult.Value);
    }

    [Fact]
    public async Task ExportCsv_ReturnsFileResult_WithData()
    {
        var mockCsvData = Encoding.UTF8.GetBytes("ID,Name,Value\n1,Item1,100\n2,Item2,200");
        var fileName = "Report.csv";

        _chartServiceMock.Setup(service => service.ExportCsvAsync(_dataTypes))
            .ReturnsAsync(mockCsvData);

        var result = await _controller.ExportCsv(_dataTypes);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType);
        Assert.Equal(fileName, fileResult.FileDownloadName);
        Assert.Equal(mockCsvData, fileResult.FileContents);
    }
}