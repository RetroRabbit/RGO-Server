using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class ChartRoleLinkUnitTest
{
    private ChartDto _chart;
    private RoleDto _role;

    public ChartRoleLinkUnitTest()
    {
        _chart = new ChartDto(1 , "Chart", "type 1", new List<string> { "label 1", "labebl 2" }, new List<int> { 1, 2 });
        _role = new RoleDto(1, "Description");
    }

    public ChartRoleLink CreateChartRoleLink(ChartDto? chart = null, RoleDto? role = null)
    {
        ChartRoleLink chartRoleLink = new ChartRoleLink
        {
            Id = 1,
            ChartId = 1,
            RoleId = 1
        };

        if (chart != null)
            chartRoleLink.Chart = new(chart);

        if (role != null)
            chartRoleLink.Role = new(role);

        return chartRoleLink;
    }

    [Fact]
    public void ChartRoleLinkTest()
    {
        var chartRoleLink = new ChartRoleLinkUnitTest();
        Assert.IsType<ChartRoleLinkUnitTest>(chartRoleLink);
        Assert.NotNull(chartRoleLink);
    }

    [Fact]
    public void ChartRoleLinkToDtoTest()
    {
        var chart = new Chart
        {
            Id = 1,
            Name = "Chart",
            Data = new List<int> { 1, 2 },
            Labels = new List<string> { "label 1", "labebl 2" },
            Type = "type 1"
        };

        var role = new Role
        {
            Id = 1,
            Description = "Description"
        };

        var chartRoleLink1 = CreateChartRoleLink();

        var dto = chartRoleLink1.ToDto();
        Assert.Null(dto.Chart);
        Assert.Null(dto.Role);

        var chartRoleLink2 = CreateChartRoleLink(chart: _chart);

        dto = chartRoleLink2.ToDto();
        Assert.Null(chartRoleLink2.Role);
        Assert.NotNull(dto.Chart);
        Assert.Equal(chartRoleLink2.ChartId, dto.Chart!.Id);

        var chartRoleLink3 = CreateChartRoleLink(role: _role);

        dto = chartRoleLink3.ToDto();
        Assert.Null(chartRoleLink3.Chart);
        Assert.NotNull(dto.Role);
        Assert.Equal(chartRoleLink3.RoleId, dto.Role!.Id);

        var chartRoleLink4 = CreateChartRoleLink(chart: _chart, role: _role);

        dto = chartRoleLink4.ToDto();
        Assert.NotNull(dto.Chart);
        Assert.NotNull(dto.Role);
        Assert.Equal(chartRoleLink4.ChartId, dto.Chart!.Id);
        Assert.Equal(chartRoleLink4.RoleId, dto.Role!.Id);

        Assert.Equal(new ChartRoleLink(chartRoleLink4.ToDto()).ChartId, chartRoleLink4.ChartId);
        Assert.Equal(new ChartRoleLink(chartRoleLink4.ToDto()).RoleId, chartRoleLink4.RoleId);
    }
}
