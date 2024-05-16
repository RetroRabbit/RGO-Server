using HRIS.Models;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class ChartRoleLinkUnitTest
{
    private readonly ChartDto _chart;
    private readonly RoleDto _role;

    public ChartRoleLinkUnitTest()
    {
        _chart = new ChartDto
        {
            Id = 1,
            Name = "Chart",
            Type = "type 1",
            DataTypes = new List<string> { "data 1", "data 2" },
            Labels = new List<string> { "label 1", "label 2" },
            Roles = new List<string> { "All" },
            Datasets = ChartDataSetTestData.chartDataSetDtoList
        };

        _role = new RoleDto { Id = 1, Description = "Description" };
    }

    public ChartRoleLink CreateChartRoleLink(ChartDto? chart = null, RoleDto? role = null)
    {
        var chartRoleLink = new ChartRoleLink
        {
            Id = 1,
            ChartId = 1,
            RoleId = 1
        };

        if (chart != null)
            chartRoleLink.Chart = new Chart(chart);

        if (role != null)
            chartRoleLink.Role = new Role(role);

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
        var chartRoleLink1 = CreateChartRoleLink();

        var dto = chartRoleLink1.ToDto();
        Assert.Null(dto.Chart);
        Assert.Null(dto.Role);

        var chartRoleLink2 = CreateChartRoleLink(_chart);

        dto = chartRoleLink2.ToDto();
        Assert.Null(chartRoleLink2.Role);
        Assert.NotNull(dto.Chart);
        Assert.Equal(chartRoleLink2.ChartId, dto.Chart!.Id);

        var chartRoleLink3 = CreateChartRoleLink(role: _role);

        dto = chartRoleLink3.ToDto();
        Assert.Null(chartRoleLink3.Chart);
        Assert.NotNull(dto.Role);
        Assert.Equal(chartRoleLink3.RoleId, dto.Role!.Id);

        var chartRoleLink4 = CreateChartRoleLink(_chart, _role);

        dto = chartRoleLink4.ToDto();
        Assert.NotNull(dto.Chart);
        Assert.NotNull(dto.Role);
        Assert.Equal(chartRoleLink4.ChartId, dto.Chart!.Id);
        Assert.Equal(chartRoleLink4.RoleId, dto.Role!.Id);

        Assert.Equal(new ChartRoleLink(chartRoleLink4.ToDto()).ChartId, chartRoleLink4.ChartId);
        Assert.Equal(new ChartRoleLink(chartRoleLink4.ToDto()).RoleId, chartRoleLink4.RoleId);
    }
}
