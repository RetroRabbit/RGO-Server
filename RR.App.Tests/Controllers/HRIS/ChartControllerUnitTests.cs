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
    private readonly ChartsController _chartsController;
    private readonly Mock<IChartService> _chartServiceMock;

    public ChartControllerUnitTests()
    {
        _chartServiceMock = new Mock<IChartService>();
        _chartsController = new ChartsController(_chartServiceMock.Object);
    }

    [Fact]
    public async Task GetAllCharts_ReturnsOk_WithCharts()
    {
        var dataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var labels = new List<string> { "Label1", "Label2", "Label3" };
        var data = new List<int> { 10, 20, 30 };
        ChartDataSetDto chartData = new ChartDataSetDto
        {
            Label = "Lable 1",
            Data = data
        };

        var mockCharts = new List<ChartDto>
        {
           new ChartDto
            {
              Id = 1,
              Name = "Disabilities",
              Type = "bar",
              DataTypes = dataTypes,
              Labels = labels,
              Roles = new List<string> { "All" },
              Datasets = new List<ChartDataSetDto>{ chartData }
           }
        };

        _chartServiceMock.Setup(service => service.GetAllCharts())
                         .ReturnsAsync(mockCharts);

        var result = await _chartsController.GetAllCharts();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockCharts, okResult.Value);
    }

    [Fact]
    public async Task GetAllCharts_ReturnsNotFound_OnException()
    {
        _chartServiceMock.Setup(service => service.GetAllCharts())
                         .ThrowsAsync(new Exception("Error message"));

        var result = await _chartsController.GetAllCharts();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Error message", notFoundResult.Value);
    }

    [Fact]
    public async Task CreateChart_ReturnsOk_OnSuccess()
    {
        var roles = new List<string> { "Developer" };
        var dataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var chartName = "Sample Chart";
        var chartType = "Bar";

        var result = await _chartsController.CreateChart(dataTypes, roles, chartName, chartType);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateChart_ReturnsNotFound_OnException()
    {
        var roles = new List<string> { "Developer" };
        var dataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var chartName = "Sample Chart";
        var chartType = "Bar";

        _chartServiceMock.Setup(service => service.CreateChart(It.IsAny<List<string>>(), It.IsAny<List<string>>(),
                                                               It.IsAny<string>(), It.IsAny<string>()))
                         .ThrowsAsync(new Exception("Error message"));

        var result = await _chartsController.CreateChart(dataTypes, roles, chartName, chartType);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Error message", notFoundResult.Value);
    }

    [Fact]
    public async Task GetChartData_ReturnsOk_WithData()
    {
        var labels = new List<string> { "Label1", "Label2", "Label3" };
        var data = new List<int> { 10, 20, 30 };
        var mockDataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var mockChartData = new ChartDataDto();
        _chartServiceMock.Setup(service => service.GetChartData(It.IsAny<List<string>>()))
                         .ReturnsAsync(mockChartData);

        var result = await _chartsController.GetChartData(mockDataTypes);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockChartData, okResult.Value);
    }

    [Fact]
    public async Task GetChartData_ReturnsNotFound_OnException()
    {
        var mockDataTypes = new List<string> { "Type1", "Type2" };
        var exceptionMessage = "An error occurred";

        _chartServiceMock.Setup(service => service.GetChartData(It.IsAny<List<string>>()))
                         .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _chartsController.GetChartData(mockDataTypes);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateChartData_ReturnsOk_WithUpdatedData()
    {
        var dataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var labels = new List<string> { "Label1", "Label2", "Label3" };
        var data = new List<int> { 10, 20, 30 };

        ChartDataSetDto chartData = new ChartDataSetDto
        {
            Label = "Lable 1",
            Data = data
        };

        var mockChartDto = new ChartDto
        {
            Id = 1,
            Name = "Disabilities",
            Type = "bar",
            DataTypes = dataTypes,
            Labels = labels,
            Datasets = new List<ChartDataSetDto> { chartData }
        };

        _chartServiceMock.Setup(service => service.UpdateChart(It.IsAny<ChartDto>()))
                         .ReturnsAsync(mockChartDto);

        var result = await _chartsController.UpdateChartData(mockChartDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockChartDto, okResult.Value);
    }

    [Fact]
    public async Task UpdateChartData_ReturnsNotFound_OnException()
    {
        var dataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var labels = new List<string> { "Label1", "Label2", "Label3" };
        var data = new List<int> { 10, 20, 30 };

        ChartDataSetDto chartData = new ChartDataSetDto
        {
            Label = "Lable 1",
            Data = data
        };

        var mockChartDto = new  ChartDto
                            {
                                Id = 1,
                                Name = "Disabilities",
                                Type = "bar",
                                DataTypes = dataTypes,
                                Labels = labels,
                                Datasets = new List<ChartDataSetDto> { chartData }
                            };
        var exceptionMessage = "An error occurred";

        _chartServiceMock.Setup(service => service.UpdateChart(It.IsAny<ChartDto>()))
                         .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _chartsController.UpdateChartData(mockChartDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteChart_ReturnsOk_WithDeletedChart()
    {
        var dataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var labels = new List<string> { "Label1", "Label2", "Label3" };
        var data = new List<int> { 10, 20, 30 };
        var chartId = 1;

        ChartDataSetDto chartData = new ChartDataSetDto
        {
            Label = "Lable 1",
            Data = data
        };

        var mockDeletedChart = new ChartDto
                                {
                                    Id = 1,
                                    Name = "Disabilities",
                                    Type = "bar",
                                    DataTypes = dataTypes,
                                    Labels = labels,
                                    Roles = new List<string> { "All" },
                                    Datasets = new List<ChartDataSetDto> { chartData }
                                };
        _chartServiceMock.Setup(service => service.DeleteChart(chartId))
                         .ReturnsAsync(mockDeletedChart);

        var result = await _chartsController.DeleteChart(chartId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockDeletedChart, okResult.Value);
    }

    [Fact]
    public async Task DeleteChart_ReturnsNotFound_OnException()
    {
        var chartId = 1;
        var exceptionMessage = "Error occurred during chart deletion";

        _chartServiceMock.Setup(service => service.DeleteChart(chartId))
                         .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _chartsController.DeleteChart(chartId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public void GetColumns_ReturnsOk_WithColumns()
    {
        var mockColumns = new List<string> { "Disabilitie", "Level", "Age" };
        _chartServiceMock.Setup(service => service.GetColumnsFromTable())
                         .Returns(mockColumns.ToArray());

        var result = _chartsController.GetColumns();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockColumns.ToArray(), okResult.Value);
    }

    [Fact]
    public void GetColumns_ReturnsNotFound_OnException()
    {
        var exceptionMessage = "Error occurred while fetching columns";

        _chartServiceMock.Setup(service => service.GetColumnsFromTable())
                         .Throws(new Exception(exceptionMessage));

        var result = _chartsController.GetColumns();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task ExportCsv_ReturnsFileResult_WithData()
    {
        var mockDataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var mockCsvData = Encoding.UTF8.GetBytes("ID,Name,Value\n1,Item1,100\n2,Item2,200");
        var fileName = "Report.csv";

        _chartServiceMock.Setup(service => service.ExportCsvAsync(mockDataTypes))
                         .ReturnsAsync(mockCsvData);

        var result = await _chartsController.ExportCsv(mockDataTypes);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType);
        Assert.Equal(fileName, fileResult.FileDownloadName);
        Assert.Equal(mockCsvData, fileResult.FileContents);
    }

    [Fact]
    public async Task ExportCsv_ReturnsNotFound_OnException()
    {
        var mockDataTypes = new List<string> { "Type1", "Type2", "Type3" };
        var exceptionMessage = "Error occurred during export";

        _chartServiceMock.Setup(service => service.ExportCsvAsync(mockDataTypes))
                         .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _chartsController.ExportCsv(mockDataTypes);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task ExportCsv_ReturnsNotFound_WhenNoData()
    {
        var mockDataTypes = new List<string> { "Type1", "Type2", "Type3" };

        byte[]? mockCsvData = null;

        _chartServiceMock.Setup(service => service.ExportCsvAsync(mockDataTypes))
                         .ReturnsAsync(mockCsvData);

        var result = await _chartsController.ExportCsv(mockDataTypes);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No data found to export.", notFoundResult.Value);
    }
}
