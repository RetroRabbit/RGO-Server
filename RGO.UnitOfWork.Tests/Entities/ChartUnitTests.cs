using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

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
        var chart = new ChartDto(
            0,
            "Name",
            "Type",
            new List<string> { "data 1", "data 2" },
            new List<string> { "Label1", "Label2" },
            new List<int> { 1, 2 });

        return new Chart(chart);
    }

    [Fact] 
    public void ChartToDtoTest()
    {
        var chartDto = new ChartDto(
            0,
            "Name",
            "Type",
            new List<string> { "data 1", "data 2" },
            new List<string> { "Label1", "Label2" },
            new List<int> { 1, 2 });

        var chart = new Chart(chartDto);

        Assert.Equivalent(chart.ToDto(), chartDto);
    }
}
