namespace HRIS.Models;

public class RoleAccessLinkDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public RoleAccessLinkDto(int Id,
                             RoleDto? Role,
                             RoleAccessDto? RoleAccess)
    {
        this.Id = Id;
        this.Role = Role;
        this.RoleAccess = RoleAccess;
    }

    public int Id { get; set; }
    public RoleDto? Role { get; set; }
    public RoleAccessDto? RoleAccess { get; set; }
}