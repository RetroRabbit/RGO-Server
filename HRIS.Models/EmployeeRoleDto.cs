using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeRoleDto
{
    [Required(ErrorMessage = "Employee Role 'Id' field is missing.")]
    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public RoleDto? Role { get; set; }
}
