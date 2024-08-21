using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class RoleAccessLinkDto
{
    [Required(ErrorMessage = "Role Access Link 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Role Access Link 'Role' field is missing.")]
    public RoleDto? Role { get; set; }
    [Required(ErrorMessage = "Role Access Link 'RoleAccess' field is missing.")]
    public RoleAccessDto? RoleAccess { get; set; }
}