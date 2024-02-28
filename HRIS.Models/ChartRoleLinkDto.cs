namespace HRIS.Models;

public class ChartRoleLinkDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public ChartRoleLinkDto(int Id,
                            ChartDto? Chart,
                            RoleDto? Role)
    {
        this.Id = Id;
        this.Chart = Chart;
        this.Role = Role;
    }

    public int Id { get; set; }
    public ChartDto? Chart { get; set; }
    public RoleDto? Role { get; set; }
}