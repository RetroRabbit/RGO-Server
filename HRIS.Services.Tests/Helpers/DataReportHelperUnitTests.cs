using HRIS.Services.Helpers;
using HRIS.Services.Interfaces.Helper;
using Moq;
using RR.UnitOfWork;
using HRIS.Models.Enums;
using HRIS.Models;
using Xunit;
using MockQueryable.Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using System.Linq.Expressions;

namespace HRIS.Services.Tests.Helpers;

public class DataReportHelperUnitTests
{
    private readonly Mock<IUnitOfWork> _db;
    private readonly IDataReportHelper _helper;
    private readonly DataReport _report;


    public DataReportHelperUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _helper = new DataReportHelper(_db.Object);
        _report = new DataReport
        {
            Code = "T001",
            Id = 1,
            Name = "Test 1",
            Status = ItemStatus.Active,
            DataReportFilter = new List<DataReportFilter>
            {
                new()
                {
                    Id = 1,
                    Table = "Employee",
                    Column = "clientAllocated",
                    Condition = "IS NULL",
                    Value = null,
                    Select = "id",
                    ReportId = 1,
                    Status = ItemStatus.Active
                }
            },
            DataReportColumns = new List<DataReportColumns>
            {
                new() {
                    Id = 1,
                    Name = "Name",
                    Prop = "Name",
                    Mapping = "Name",
                    Sequence = 0,
                    IsCustom = false,
                    FieldType = null
                },
                new() {
                    Id = 2,
                    Name = "Surname",
                    Prop = "Surname",
                    Mapping = "Surname",
                    Sequence = 1,
                    IsCustom = false,
                    FieldType = null
                },
                new() {
                    Id = 3,
                    Name = "Role",
                    Prop = "Role",
                    Mapping = "EmployeeType.Name",
                    Sequence = 2,
                    IsCustom = false,
                    FieldType = null
                },
                new() {
                    Id = 4,
                    Name = "Level",
                    Prop = "Level",
                    Mapping = "Level",
                    Sequence = 3,
                    IsCustom = false,
                    FieldType = null
                },
                new() {
                    Id = 5,
                    Name = "Location",
                    Prop = "Location",
                    Mapping = "PhysicalAddress.SuburbOrDistrict",
                    Sequence = 4,
                    IsCustom = false,
                    FieldType = null
                },
                new() {
                    Id = 6,
                    Name = "NQF Level",
                    Prop = "NqfLevel",
                    Mapping = "nqf",
                    Sequence = 5,
                    IsCustom = true,
                    FieldType = DataReportCustom.EmployeeData
                },
                new() {
                    Id = 7,
                    Name = "Open Source",
                    Prop = "OpenSource",
                    Mapping = "OpenSource",
                    Sequence = 8,
                    IsCustom = true,
                    FieldType = DataReportCustom.Checkbox
                },
                new() {
                    Id = 8,
                    Name = "Notes",
                    Prop = "Notes",
                    Mapping = "Notes",
                    Sequence = 7,
                    IsCustom = true,
                    FieldType = DataReportCustom.Text
                }
            },
            DataReportValues = new List<DataReportValues>
            {
                new()
                {
                    Id = 1,
                    ReportId = 1,
                    EmployeeId = 1,
                    Input = "Test",
                    ColumnId = 8
                }
            }
        };
    }

    [Fact]
    public async Task GetReport()
    {
        var dataReport = new List<DataReport> { _report };

        _db.Setup(u => u.DataReport.Get(It.IsAny<Expression<Func<DataReport, bool>>>())).Returns(dataReport.AsQueryable().BuildMock());

        var result = await _helper.GetReport(It.IsAny<string>());

        Assert.NotNull(result);
        Assert.IsType<DataReport>(result);
        _db.Verify(u => u.DataReport.Get(It.IsAny<Expression<Func<DataReport, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeIdListForReport()
    {
        _db.Setup(u => u.RawSqlForIntList("SELECT \"id\" FROM \"Employee\"", "id")).ReturnsAsync(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        _db.Setup(u => u.RawSqlForIntList("SELECT \"id\" FROM \"Employee\" WHERE  \"clientAllocated\" IS NULL ", "id")).ReturnsAsync(new List<int> { 3, 4, 6, 11 });

        var result = await _helper.GetEmployeeIdListForReport(_report);

        Assert.NotNull(result);
        Assert.IsType<List<int>>(result);
        Assert.Equal(new List<int> { 3, 4, 6 }, result);
    }

    [Fact]
    public async Task GetEmployeeIdListForReport_Fail()
    {
        var exception =
            await Assert.ThrowsAsync<Exception>(async () => await _helper.GetEmployeeIdListForReport(new DataReport { Code = "Failing Report"}));

        Assert.Equal("Report 'Failing Report' has no filters", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeData()
    {
        var employeeList = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

        _db.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.AsQueryable().BuildMock());

        var result = await _helper.GetEmployeeData(It.IsAny<List<int>>());

        Assert.NotNull(result);
        Assert.IsType<List<Employee>>(result);
        _db.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Once);
    }

    [Fact]
    public void MapEmployeeData_Fail()
    {
        var exception =
            Assert.Throws<Exception>(() => _helper.MapEmployeeData(new DataReport { Code = "Failing Report" }, It.IsAny<List<Employee>>()));

        Assert.Equal("Report 'Failing Report' has no columns", exception.Message);
    }

    [Fact]
    public void MapEmployeeData()
    {
        var employees = new List<Employee>
        {
            new()
            {
                Id = 1,
                Name = "Jane",
                Surname = "Doe",
                EmployeeType = new EmployeeType
                {
                    Name = "Developer"
                },
                Level = 6,
                PhysicalAddress = new EmployeeAddress
                {
                    SuburbOrDistrict = "Gotham City"
                },
                EmployeeData = new List<EmployeeData>
                {
                    new()
                    {
                        FieldCode = new FieldCode
                        {
                            Code = "nqf",
                        },
                        Value = "NQF 99"
                    }
                }
            }
        };
        var result = _helper.MapEmployeeData(_report, employees);
        var expected = new Dictionary<string, object?>
        {
            {"Id", 1},
            {"Name", "Jane"},
            {"Surname", "Doe"},
            {"Role", "Developer"},
            {"Level", 6},
            {"Location", "Gotham City"},
            {"NqfLevel", "NQF 99"},
            {"OpenSource", false},
            {"Notes", "Test"},
        };

        Assert.NotNull(result);
        Assert.IsType<List<Dictionary<string, object?>>>(result);
        Assert.Equivalent(new List<Dictionary<string, object?>> { expected }, result);
    }

    [Fact]
    public void MapReportColumns()
    {
        var result = _helper.MapReportColumns(_report);

        Assert.NotNull(result);
        Assert.IsType<List<DataReportColumnsDto>>(result);
        var expected = new List<DataReportColumnsDto>
        {
            new() {
                Id = 1,
                Name = "Name",
                Prop = "Name",
                Sequence = 0,
                IsCustom = false,
                FieldType = null
            },
            new() {
                Id = 2,
                Name = "Surname",
                Prop = "Surname",
                Sequence = 1,
                IsCustom = false,
                FieldType = null
            },
            new() {
                Id = 3,
                Name = "Role",
                Prop = "Role",
                Sequence = 2,
                IsCustom = false,
                FieldType = null
            },
            new() {
                Id = 4,
                Name = "Level",
                Prop = "Level",
                Sequence = 3,
                IsCustom = false,
                FieldType = null
            },
            new() {
                Id = 5,
                Name = "Location",
                Prop = "Location",
                Sequence = 4,
                IsCustom = false,
                FieldType = null
            },
            new() {
                Id = 6,
                Name = "NQF Level",
                Prop = "NqfLevel",
                Sequence = 5,
                IsCustom = false,
                FieldType = null
            },
            new() {
                Id = 8,
                Name = "Notes",
                Prop = "Notes",
                Sequence = 7,
                IsCustom = true,
                FieldType = "Text"
            },
            new() {
                Id = 7,
                Name = "Open Source",
                Prop = "OpenSource",
                Sequence = 8,
                IsCustom = true,
                FieldType = "Checkbox"
            }
        };
        Assert.Equivalent(expected, result);
    }
}