namespace HRIS.Models;

public class EmployeeRoleDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeRoleDto(int Id,
                           EmployeeDto? Employee,
                           RoleDto? Role)
    {
        this.Id = Id;
        this.Employee = Employee;
        this.Role = Role;
    }

    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public RoleDto? Role { get; set; }
}