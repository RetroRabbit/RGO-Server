namespace HRIS.Models;

public class EmployeeRoleDto
{
    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public RoleDto? Role { get; set; }
}
