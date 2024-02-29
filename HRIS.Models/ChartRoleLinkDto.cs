namespace HRIS.Models;

public class ChartRoleLinkDto
{
    public int Id { get; set; }
    public ChartDto? Chart { get; set; }
    public RoleDto? Role { get; set; }
}
