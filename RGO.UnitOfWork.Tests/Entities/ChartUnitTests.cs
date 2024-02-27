using HRIS.Models;
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
        var chartDto = new ChartDto
        {
            Id = 0,
            Name = "Name",
            Type = "Type",
            DataTypes = new List<string> { "data 1", "data 2" },
            Labels = new List<string> { "Label1", "Label2" },
            Data = new List<int> { 1, 2 }
        };

        return new Chart(chartDto);

    }

    [Fact]
    public void ChartToDtoTest()
    {
        var chartDto = new ChartDto
        {
            Id = 0,
            Name = "Name",
            Type = "Type",
            DataTypes = new List<string> { "data 1", "data 2" },
            Labels = new List<string> { "Label1", "Label2" },
            Data = new List<int> { 1, 2 }
        };

        var chart = new Chart(chartDto);

        Assert.Equivalent(chart.ToDto(), chartDto);
    }
}
