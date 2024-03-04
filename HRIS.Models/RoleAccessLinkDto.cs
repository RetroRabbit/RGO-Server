namespace HRIS.Models;

public class RoleAccessLinkDto
{
    public int Id { get; set; }
    public RoleDto? Role { get; set; }
    public RoleAccessDto? RoleAccess { get; set; }
}