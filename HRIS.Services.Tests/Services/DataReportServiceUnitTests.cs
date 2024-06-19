using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Update;
using HRIS.Services.Interfaces;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class DataReportServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _db;
    private readonly Mock<IDataReportHelper> _helper;
    private readonly IDataReportService _service;

    public DataReportServiceUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _helper = new Mock<IDataReportHelper>();
        _service = new DataReportService(_db.Object, _helper.Object);
    }

    [Fact]
    public async Task GetDataReportList()
    {
        var dataReports = new List<DataReportDto>
        {
            new()
            {
                Code = "T001",
                Id = 1,
                Name = "Test 1",
                Status = ItemStatus.Active
            }
        };

        _db.Setup(y => y.DataReport.GetAll(x => x.Status == ItemStatus.Active)).ReturnsAsync(dataReports);
        var result = await _service.GetDataReportList();

        Assert.Equal(dataReports, result);
    }

    [Fact]
    public async Task GetDataReport()
    {
        var report = new DataReport { Id = 1, Name = "Test Report" };
        _helper.Setup(x => x.GetReport(It.IsAny<string>())).ReturnsAsync(report);
        _helper.Setup(x => x.GetEmployeeIdListForReport(It.IsAny<DataReport>())).ReturnsAsync(new List<int>());
        _helper.Setup(x => x.GetEmployeeData(It.IsAny<List<int>>())).ReturnsAsync(new List<Employee>());
        _helper.Setup(x => x.MapEmployeeData(It.IsAny<DataReport>(), It.IsAny<List<Employee>>()))
               .Returns(new List<Dictionary<string, object?>>());
        _helper.Setup(x => x.MapReportColumns(It.IsAny<DataReport>())).Returns(new List<DataReportColumnsDto>());

        var result = await _service.GetDataReport("TEST");

        Assert.Equivalent(new
        {
            ReportName = "Test Report",
            ReportId = 1,
            Columns = new List<DataReportColumnsDto>(),
            Data = new List<Dictionary<string, object?>>()
        }, result);
    }

    [Fact]
    public async Task UpdateReportInput()
    {
        var input = new UpdateReportCustomValue
        {
            ColumnId = 1,
            EmployeeId = 2,
            ReportId = 3,
            Input = "some Value"
        };
        _db.Setup(y => y.DataReportValues.FirstOrDefault(x =>
               x.ReportId == input.ReportId && x.ColumnId == input.ColumnId && x.EmployeeId == input.EmployeeId))
           .ReturnsAsync(new DataReportValuesDto());

        await _service.UpdateReportInput(input);

        _db.Verify(x => x.DataReportValues.Update(It.IsAny<DataReportValues>()), Times.Once);
    }

    [Fact]
    public async Task AddReportInput()
    {
        var input = new UpdateReportCustomValue
        {
            ColumnId = 1,
            EmployeeId = 2,
            ReportId = 3,
            Input = "some Value"
        };

        _db.Setup(y => y.DataReportValues.FirstOrDefault(x =>
               x.ReportId == input.ReportId && x.ColumnId == input.ColumnId && x.EmployeeId == input.EmployeeId))
           .ReturnsAsync((DataReportValuesDto)null!);

        await _service.UpdateReportInput(input);

        _db.Verify(x => x.DataReportValues.Add(It.IsAny<DataReportValues>()), Times.Once);
    }
}