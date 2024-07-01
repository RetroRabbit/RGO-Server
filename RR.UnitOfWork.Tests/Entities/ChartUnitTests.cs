using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class ChartUnitTests
{
    [Fact]
    public void chartTest()
    {
        var chart = new Chart();
        Assert.IsType<Chart>(chart);
        Assert.NotNull(chart);
    }

    public Chart CreateTestChartDto()
    {
        var chartDto = new Chart
        {
            Id = 0,
            Name = "Name",
            Type = "Type",
            DataTypes = new List<string> { "data 1", "data 2" },
            Labels = new List<string> { "Label1", "Label2" },
            Roles = new List<string> { "All" },
            Datasets = ChartDataSetTestData.ChartDataSetList
        };

        return chartDto;
    }

    [Fact]
    public void ChartToDtoTest()
    {
        var chartDto = new Chart
        {
            Id = 0,
            Name = "Name",
            Type = "Type",
            DataTypes = new List<string> { "data 1", "data 2" },
            Labels = new List<string> { "Label1", "Label2" },
            Roles = new List<string> { "All" },
            Datasets = ChartDataSetTestData.ChartDataSetList
        };

        var chart = chartDto;

        Assert.Equivalent(chart.ToDto(), chartDto);
    }
}
